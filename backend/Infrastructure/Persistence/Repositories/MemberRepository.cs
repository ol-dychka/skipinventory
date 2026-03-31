using System;
using Application.Interfaces;
using Domain;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MemberRepository(PsqlDbContext context) : IMemberRepository
{
    private readonly PsqlDbContext _context = context;

    public void Add(OrganizationMember member)
    {
        _context.OrganizationMembers.Add(member);
    }

    public Task<OrganizationMember?> GetAsync(
        string organizationId,
        string userId,
        CancellationToken cancellationToken
    )
    {
        return _context.OrganizationMembers.FirstOrDefaultAsync(
            om => om.OrganizationId == organizationId && om.UserId == userId,
            cancellationToken
        );
    }
}
