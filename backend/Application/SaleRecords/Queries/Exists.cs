using System;
using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.SaleRecords.Queries;

public class Exists
{
    public record Query(string OrganizationId, DateTime Date) : IRequest<Result<bool>>;

    public class Handler(ISaleRecordRepository repository) : IRequestHandler<Query, Result<bool>>
    {
        public async Task<Result<bool>> Handle(Query request, CancellationToken cancellationToken)
        {
            var exists = await repository.ExistsFromDateAsync(
                request.OrganizationId,
                request.Date,
                cancellationToken
            );

            return Result<bool>.Success(exists);
        }
    }
}
