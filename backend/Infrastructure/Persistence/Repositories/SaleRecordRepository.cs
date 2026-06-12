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
        DateTime date,
        CancellationToken cancellationToken
    )
    {
        DateTime start = date.Date;
        DateTime end = date.Date.AddDays(1);

        return _context.SaleRecords.AnyAsync(
            sr => sr.OrganizationId == organizationId && sr.Date >= start && sr.Date < end,
            cancellationToken
        );
    }

    public Task<List<SaleRecord>> GetAllAsync(
        string organizationId,
        CancellationToken cancellationToken
    )
    {
        // safety net; mock data is generated for only 21 days
        var startDate = DateTime.UtcNow.AddDays(-21);

        return _context
            .SaleRecords.Where(sr => sr.OrganizationId == organizationId && sr.Date > startDate)
            .ToListAsync(cancellationToken);
    }

    public Task<List<SaleRecord>> GetFromDateAsync(
        string organizationId,
        DateTime date,
        CancellationToken cancellationToken
    )
    {
        DateTime start = date.Date;
        DateTime end = date.Date.AddDays(1);

        return _context
            .SaleRecords.Where(sr =>
                sr.OrganizationId == organizationId && sr.Date >= start && sr.Date < end
            )
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
                && sr.Date > DateTime.UtcNow.AddDays(-1 * numberOfDays)
            )
            .ToListAsync(cancellationToken);
    }
}
