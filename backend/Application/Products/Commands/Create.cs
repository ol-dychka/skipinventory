using System;
using System.Text.RegularExpressions;
using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.StaticClasses;
using MediatR;

namespace Application.Products.Commands;

public class Create
{
    public class Command : IRequest<Result<Product>>
    {
        public required string UserId { get; set; }
        public required string Name { get; set; }
        public string? Sku { get; set; }
        public required string Vendor { get; set; }
        public required string OrganizationId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int CurrentStock { get; set; }
        public int? ReorderPoint { get; set; }
        public int BaseReorderQuantity { get; set; }
        public int? DeliveryDelay { get; set; }
        public string? Category { get; set; }
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

            var generatedSku = GenerateSku(request.Name, request.Sku);

            var product = new Product(
                request.Name,
                generatedSku,
                request.Vendor,
                request.OrganizationId,
                request.CostPrice,
                request.SalePrice,
                request.CurrentStock,
                request.ReorderPoint ?? request.BaseReorderQuantity,
                request.BaseReorderQuantity,
                request.DeliveryDelay ?? 0,
                request.Category
            );

            productRepository.Add(product);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Product>.Success(product);
        }
    }

    private static string GenerateSku(string name, string? sku)
    {
        string suffix = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();

        if (!string.IsNullOrWhiteSpace(sku))
        {
            return $"{sku.Trim().ToUpperInvariant()}-{suffix}";
        }

        // replaces everything that's not a capital, number or some kind of space
        string normalized = Regex.Replace(name.Trim().ToUpperInvariant(), @"[^A-Z0-9\s]", "");
        normalized = Regex.Replace(normalized, @"\s+", " ").Trim();

        // get first 3 letters of first 3 words
        string prefix = string.Join(
            "-",
            normalized.Split(" ").Take(3).Select(w => w.Length >= 3 ? w[..3] : w)
        );

        return $"{prefix}-{suffix}";
    }
}
