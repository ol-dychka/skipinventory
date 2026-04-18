using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Products.Queries;

public class List
{
    public record Query(string OrganizationId) : IRequest<Result<List<Product>>>;

    public class Handler(IProductRepository repository)
        : IRequestHandler<Query, Result<List<Product>>>
    {
        public async Task<Result<List<Product>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var products = await repository.GetAllAsync(request.OrganizationId, cancellationToken);

            return Result<List<Product>>.Success(products);
        }
    }
}
