using System;
using Domain;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Organizations.Queries;

public class GetOrganizationList
{
    public class Query : IRequest<List<Organization>> {}

    public class Handler(PsqlDbContext context) : IRequestHandler<Query, List<Organization>>
    {
        public async Task<List<Organization>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.Organizations.ToListAsync(cancellationToken);
        }
    }
}
