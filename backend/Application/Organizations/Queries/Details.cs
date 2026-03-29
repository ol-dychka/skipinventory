using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Organizations.Queries;

public class Details
{
    public record Query(string Id) : IRequest<Result<Organization>>;

    public class Handler(IOrganizationRepository repository)
        : IRequestHandler<Query, Result<Organization>>
    {
        public async Task<Result<Organization>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var organization = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (organization == null)
                return Result<Organization>.Failure("Couldn't find this organization");

            return Result<Organization>.Success(organization);
        }
    }
}
