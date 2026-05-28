using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.StaticClasses;
using MediatR;

namespace Application.Organizations.Commands;

public class Resolve
{
    public record Command(string RequestId, bool Decision, string ResolverId)
        : IRequest<Result<Response>>;

    public record Response(string UserId, string OrganizationName);

    public class Handler(
        IUserRepository userRepository,
        IJoinRequestRepository requestRepository,
        IMemberRepository memberRepository,
        IUnitOfWork unitOfWork
    ) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var joinRequest = await requestRepository.GetByIdAsync(
                request.RequestId,
                cancellationToken
            );
            if (joinRequest == null)
                return Result<Response>.Failure("request does not exist");

            var resolver = await userRepository.GetByIdAsync(request.ResolverId, cancellationToken);
            if (resolver == null)
                return Result<Response>.Failure("Current user does not exist");

            var hasRight = resolver.Memberships.Any(m =>
                m.OrganizationId == joinRequest.OrganizationId
                && UserRole.HasResolveJoinRights(m.Role)
            );
            if (!hasRight)
                return Result<Response>.Failure("Current user has no rights to resolve a request");

            // true or false. method return Result<Response> for now because request is resolved anyway
            // later with SignalR, user will get a toast with resolve information
            if (request.Decision)
            {
                var membership = new OrganizationMember(
                    joinRequest.UserId,
                    joinRequest.OrganizationId,
                    UserRole.Employee
                );
                memberRepository.Add(membership);

                joinRequest.Status = JoinRequestStatus.Accepted;
            }
            else
            {
                joinRequest.Status = JoinRequestStatus.Rejected;
            }

            joinRequest.ResolvedAt = DateTime.UtcNow;
            joinRequest.ResolverId = request.ResolverId;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Response>.Success(
                new Response(joinRequest.UserId, joinRequest.Organization.Name)
            );
        }
    }
}
