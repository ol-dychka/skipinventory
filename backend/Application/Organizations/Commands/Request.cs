using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.StaticClasses;
using MediatR;

namespace Application.Organizations.Commands;

public class Request
{
    public record Command(string OrganizationId, string UserId) : IRequest<Result<Unit>>;

    public class Handler(
        IOrganizationRepository organizationRepository,
        IJoinRequestRepository requestRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork
    ) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                return Result<Unit>.Failure("User not found");

            var organization = await organizationRepository.GetByIdAsync(
                request.OrganizationId,
                cancellationToken
            );
            if (organization == null)
                return Result<Unit>.Failure("Organization not found");

            if (user.Memberships.Any(m => m.OrganizationId == organization.Id))
                return Result<Unit>.Failure("Already has a membership in this Organization");

            var existing = await requestRepository.GetByCredentialsAsync(
                user.Id,
                organization.Id,
                cancellationToken
            );
            // request works only if:
            // 1. first apply
            // 2. previously denied
            if (existing != null)
                if (
                    existing.Status == JoinRequestStatus.Accepted
                    || existing.Status == JoinRequestStatus.Pending
                )
                    return Result<Unit>.Failure("Already applied to this Organization");

            var joinRequest = new JoinRequest(request.UserId, request.OrganizationId);

            requestRepository.Add(joinRequest);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
