using System;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class OrganizationRepository(PsqlDbContext context) : IOrganizationRepository
{
    private readonly PsqlDbContext _context = context;

    public void Add(Organization organization)
    {
        _context.Organizations.Add(organization);
    }

    public void Delete(Organization organization)
    {
        _context.Organizations.Remove(organization);
    }

    public Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken)
    {
        return _context.Organizations.AnyAsync(o => o.Name == name, cancellationToken);
    }

    public Task<List<Organization>> GetAllAsync(CancellationToken cancellationToken)
    {
        return _context.Organizations.ToListAsync(cancellationToken);
    }

    public Task<Organization?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _context
            .Organizations.Include(o => o.Creator)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
}
