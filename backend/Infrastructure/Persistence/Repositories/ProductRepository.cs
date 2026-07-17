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

    public Task<Product?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<Product?> GetBySkuAsync(
        string sku,
        string organizationId,
        CancellationToken cancellationToken
    )
    {
        return _context.Products.FirstOrDefaultAsync(
            p => p.Sku == sku && p.OrganizationId == organizationId,
            cancellationToken
        );
    }

    public Task<Dictionary<string, Product>> GetBySkusAsync(
        string organizationId,
        List<string> skus,
        CancellationToken cancellationToken
    )
    {
        return _context
            .Products.Where(p => p.OrganizationId == organizationId && skus.Contains(p.Sku))
            .ToDictionaryAsync(p => p.Sku, cancellationToken);
    }
}
