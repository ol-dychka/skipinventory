using System;
using Application.Interfaces;

namespace Infrastructure.Persistence.Repositories;

public class UnitOfWork(PsqlDbContext context) : IUnitOfWork
{
    private readonly PsqlDbContext _context = context;

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
