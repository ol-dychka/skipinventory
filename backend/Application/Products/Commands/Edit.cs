using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.StaticClasses;
using MediatR;

namespace Application.Products.Commands;

public class Edit
{
    public class Command : IRequest<Result<Product>>
    {
        public required string UserId { get; set; }
        public required string ProductId { get; set; }
        public required string Name { get; set; }
        public required string Sku { get; set; }
        public required string Vendor { get; set; }
        public required string OrganizationId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int CurrentStock { get; set; }
        public int ReorderPoint { get; set; }
        public int BaseReorderQuantity { get; set; }
        public int DeliveryDelay { get; set; }
        public string? Category { get; set; }
        public bool IsActive { get; set; }
    }

    public class Handler(
        IUserRepository userRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork
    ) : IRequestHandler<Command, Result<Product>>
    {
        public async Task<Result<Product>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                return Result<Product>.Failure("User not found");

            var hasRight = user.Memberships.Any(m =>
                m.OrganizationId == request.OrganizationId && UserRole.HasResolveJoinRights(m.Role)
            );
            if (!hasRight)
                return Result<Product>.Failure("Current user has no rights to perform this action");

            var product = await productRepository.GetByIdAsync(
                request.ProductId,
                cancellationToken
            );
            if (product == null)
                return Result<Product>.Failure("Error finding the product");

            product.Name = request.Name;
            product.Sku = request.Sku;
            product.Vendor = request.Vendor;
            product.OrganizationId = request.OrganizationId;
            product.CostPrice = request.CostPrice;
            product.SalePrice = request.SalePrice;
            product.CurrentStock = request.CurrentStock;
            product.ReorderPoint = request.ReorderPoint;
            product.BaseReorderQuantity = request.BaseReorderQuantity;
            product.DeliveryDelay = request.DeliveryDelay;
            product.Category = request.Category;
            product.IsActive = request.IsActive;
            product.UpdatedAt = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Product>.Success(product);
        }
    }
}
