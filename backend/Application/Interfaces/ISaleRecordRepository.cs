using System;
using Domain;

namespace Application.Interfaces;

public interface ISaleRecordRepository
{
    void Add(SaleRecord saleRecord);
    Task<List<SaleRecord>> GetFromDateAsync(
        string organizationId,
        DateOnly date,
        CancellationToken cancellationToken
    );

    Task<bool> ExistsFromDateAsync(
        string organizationId,
        DateOnly date,
        CancellationToken cancellationToken
    );

    Task<List<SaleRecord>> GetFromDateRangeAsync(
        string organizationId,
        int NumberOfDays,
        CancellationToken cancellationToken
    );
}
