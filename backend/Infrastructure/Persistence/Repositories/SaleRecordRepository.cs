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
        var floor = date.Date;
        var ceiling = date.Date.AddDays(1);
        return _context.SaleRecords.AnyAsync(
            sr => sr.OrganizationId == organizationId && date >= floor && date < ceiling,
            cancellationToken
        );
    }

    public Task<List<SaleRecord>> GetFromDateAsync(
        string organizationId,
        DateTime date,
        CancellationToken cancellationToken
    )
    {
        var floor = date.Date;
        var ceiling = date.Date.AddDays(1);
        return _context
            .SaleRecords.Where(sr =>
                sr.OrganizationId == organizationId && date >= floor && date < ceiling
            )
            .ToListAsync(cancellationToken);
    }
}
