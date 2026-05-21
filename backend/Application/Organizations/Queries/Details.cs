using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Organizations.Queries;

public class Details
{
    public record Query(string OrganizationId, string UserId) : IRequest<Result<Organization>>;

    public class Handler(IOrganizationRepository repository)
        : IRequestHandler<Query, Result<Organization>>
    {
        public async Task<Result<Organization>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var organization = await repository.GetByIdAsync(
                request.OrganizationId,
                cancellationToken
            );
            if (organization == null)
                return Result<Organization>.Failure("Couldn't find this organization");

            if (!organization.Members.Any(m => m.UserId == request.UserId))
                return Result<Organization>.Failure("Current user is not a member");

            return Result<Organization>.Success(organization);
        }
    }
}
