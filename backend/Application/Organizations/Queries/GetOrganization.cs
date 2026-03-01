using System;
using Domain;
using Infrastructure.Persistence;
using MediatR;

namespace Application.Organizations.Queries;

public class GetOrganization
{
    public class Query : IRequest<Organization>
    {
        public required string Id { get; set; }
    }

    public class Handler(PsqlDbContext context) : IRequestHandler<Query, Organization>
    {
        public async Task<Organization> Handle(Query request, CancellationToken cancellationToken)
        {
            var organization = await context.Organizations
                .FindAsync([request.Id], cancellationToken)
                    ?? throw new Exception("Not Found");
            return organization;
        }
    }
}
