using System;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Organizations.Queries;

public class GetOrganizationList
{
    public record Query : IRequest<List<Organization>>;

    public class Handler(IOrganizationRepository repository) : IRequestHandler<Query, List<Organization>>
    {
        public async Task<List<Organization>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await repository.GetAllAsync(cancellationToken);
        }
    }
}
