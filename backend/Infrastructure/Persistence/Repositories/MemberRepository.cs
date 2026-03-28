using System;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MemberRepository(PsqlDbContext context) : IMemberRepository
{
    private readonly PsqlDbContext _context = context;

    public void Add(OrganizationMember member)
    {
        _context.OrganizationMembers.Add(member);
    }

    public Task<OrganizationMember?> GetByOrgIdAsync(string id, CancellationToken cancellationToken)
    {
        return _context.OrganizationMembers.FirstOrDefaultAsync(
            om => om.OrganizationId == id,
            cancellationToken
        );
    }
}
