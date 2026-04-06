using System;
using Application.Core;
using Application.Interfaces;
using Domain;
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

            var joinRequest = new JoinRequest(request.UserId, request.OrganizationId);

            requestRepository.Add(joinRequest);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
