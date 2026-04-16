using System;
using Application.Interfaces;
using Domain;

namespace Infrastructure.Persistence.Repositories;

public class ProductRepository(PsqlDbContext context) : IProductRepository
{
    private readonly PsqlDbContext _context = context;

    public void Add(Product product)
    {
        _context.Products.Add(product);
    }

    public Task<List<Product>> GetAllAsync(
        string organizationId,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}
