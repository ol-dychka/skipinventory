using System;
using Domain;

namespace Application.Interfaces;

public interface ISaleRecordRepository
{
    void Add(SaleRecord saleRecord);
    Task<List<SaleRecord>> GetFromDateAsync(
        string organizationId,
        DateTime date,
        CancellationToken cancellationToken
    );
    Task<bool> ExistsFromDateAsync(
        string organizationId,
        DateTime date,
        CancellationToken cancellationToken
    );
}
