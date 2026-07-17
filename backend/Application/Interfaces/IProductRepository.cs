using System;
using Domain;
using MediatR;

namespace Application.Interfaces;

public interface IProductRepository
{
    void Add(Product product);
    Task<List<Product>> GetAllAsync(string organizationId, CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Product?> GetBySkuAsync(
        string sku,
        string organizationId,
        CancellationToken cancellationToken
    );
    Task<Dictionary<string, Product>> GetBySkusAsync(
        string organizationId,
        List<string> skus,
        CancellationToken cancellationToken
    );
}
