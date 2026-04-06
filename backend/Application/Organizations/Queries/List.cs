using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Organizations.Queries;

public class List
{
    public record Query : IRequest<Result<List<Organization>>>;

    public class Handler(IOrganizationRepository repository)
        : IRequestHandler<Query, Result<List<Organization>>>
    {
        public async Task<Result<List<Organization>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var organizations = await repository.GetAllAsync(cancellationToken);

            return Result<List<Organization>>.Success(organizations);
        }
    }
}
