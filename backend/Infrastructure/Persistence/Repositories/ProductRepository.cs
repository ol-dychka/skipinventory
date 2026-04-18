using System;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

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
        return _context
            .Products.Where(p => p.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
    }
}
