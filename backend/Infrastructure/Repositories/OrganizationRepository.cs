using System;
using Application.Interfaces;
using Domain;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrganizationRepository(PsqlDbContext context) : IOrganizationRepository
{
    private readonly PsqlDbContext _context = context;

    public Task Add(Organization organization)
    {
        _context.Organizations.Add(organization);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Organization>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Organizations
            .ToListAsync(cancellationToken);
    }

    public async Task<Organization?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _context.Organizations
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public Task Delete(Organization organization)
    {
        _context.Organizations.Remove(organization);
        return Task.CompletedTask;
    }
}
