using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.StaticClasses;
using MediatR;

namespace Application.Products.Commands;

public class Create
{
    public class Command : IRequest<Result<Unit>>
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
    ) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                return Result<Unit>.Failure("User not found");

            var hasRight = user.Memberships.Any(m =>
                m.OrganizationId == request.OrganizationId && UserRole.HasResolveJoinRights(m.Role)
            );
            if (!hasRight)
                return Result<Unit>.Failure("Current user has no rights to perform this action");

            var product = new Product(
                request.Name,
                request.Sku ?? request.Name,
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

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
