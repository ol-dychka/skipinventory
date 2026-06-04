using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.StaticClasses;
using MediatR;

namespace Application.Organizations.Commands;

public class Promote
{
    public record Command(
        string TargetId,
        string ResolverId,
        string OrganizationId,
        bool IsPromotion
    ) : IRequest<Result<Response>>;

    public record Response(string NewRole);

    public class Handler(IMemberRepository memberRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var resolver = await memberRepository.GetAsync(
                request.OrganizationId,
                request.ResolverId,
                cancellationToken
            );
            if (resolver == null)
                return Result<Response>.Failure("Current user does not exist");

            if (!UserRole.HasPromoteRights(resolver.Role))
                return Result<Response>.Failure("Current user has no rights to resolve a request");

            var member = await memberRepository.GetAsync(
                request.OrganizationId,
                request.TargetId,
                cancellationToken
            );
            if (member == null)
                return Result<Response>.Failure("Error finding the user for promotion");

            if (request.IsPromotion)
                member.Role = UserRole.Manager;
            else
                member.Role = UserRole.Employee;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Response>.Success(new Response(member.Role));
        }
    }
}
