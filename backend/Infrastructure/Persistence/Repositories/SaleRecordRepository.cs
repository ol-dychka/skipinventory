using System;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SaleRecordRepository(PsqlDbContext context) : ISaleRecordRepository
{
    private readonly PsqlDbContext _context = context;

    public void Add(SaleRecord saleRecord)
    {
        _context.SaleRecords.Add(saleRecord);
    }

    public Task<bool> ExistsFromDateAsync(
        string organizationId,
        DateOnly date,
        CancellationToken cancellationToken
    )
    {
        return _context.SaleRecords.AnyAsync(
            sr => sr.OrganizationId == organizationId && sr.Date == date,
            cancellationToken
        );
    }

    public Task<List<SaleRecord>> GetFromDateAsync(
        string organizationId,
        DateOnly date,
        CancellationToken cancellationToken
    )
    {
        return _context
            .SaleRecords.Where(sr => sr.OrganizationId == organizationId && sr.Date == date)
            .ToListAsync(cancellationToken);
    }

    public Task<List<SaleRecord>> GetFromDateRangeAsync(
        string organizationId,
        int numberOfDays,
        CancellationToken cancellationToken
    )
    {
        return _context
            .SaleRecords.Where(sr =>
                sr.OrganizationId == organizationId
                && sr.Date > DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1 * numberOfDays))
            )
            .ToListAsync(cancellationToken);
    }
}
