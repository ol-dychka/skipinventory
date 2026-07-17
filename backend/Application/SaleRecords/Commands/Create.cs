using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.StaticClasses;
using MediatR;

namespace Application.SaleRecords.Commands;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public List<CreateSaleRecord> Data { get; set; } = [];
        public DateTime? Date { get; set; }
        public required string UserId { get; set; }
        public required string OrganizationId { get; set; }
    }

    public record CreateSaleRecord(string Id, int Quantity, string Sku);

    public class Handler(
        IUserRepository userRepository,
        ISaleRecordRepository saleRepository,
        IUnitOfWork unitOfWork,
        IProductRepository productRepository
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

            var skus = request.Data.Select(r => r.Sku).ToList();
            var products = await productRepository.GetBySkusAsync(
                request.OrganizationId,
                skus,
                cancellationToken
            );

            foreach (var record in request.Data)
            {
                var newSaleRecord = new SaleRecord(
                    record.Quantity >= 0 ? record.Quantity : 0,
                    record.Sku,
                    request.OrganizationId,
                    record.Id,
                    record.Quantity < 0 ? record.Quantity : 0,
                    request.Date ?? DateTime.UtcNow
                );

                saleRepository.Add(newSaleRecord);

                if (products.TryGetValue(record.Sku, out var product))
                {
                    product.CurrentStock -= record.Quantity;
                }
                else
                {
                    Console.WriteLine("Error updating product stock");
                }
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
