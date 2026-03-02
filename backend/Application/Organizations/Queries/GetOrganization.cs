using System;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Organizations.Queries;

public class GetOrganization
{
    public record Query(string Id) : IRequest<Organization>;

    public class Handler(IOrganizationRepository repository) : IRequestHandler<Query, Organization>
    {
        public async Task<Organization> Handle(Query request, CancellationToken cancellationToken)
        {
            var organization = await repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new Exception("Cannot find this organization");

            return organization;
        }
    }
}
