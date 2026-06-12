using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.SaleRecords.Queries;

public class All
{
    public record Query(string OrganizationId) : IRequest<Result<List<SaleRecord>>>;

    public class Handler(ISaleRecordRepository repository)
        : IRequestHandler<Query, Result<List<SaleRecord>>>
    {
        public async Task<Result<List<SaleRecord>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var saleRecords = await repository.GetAllAsync(
                request.OrganizationId,
                cancellationToken
            );

            return Result<List<SaleRecord>>.Success(saleRecords);
        }
    }
}
