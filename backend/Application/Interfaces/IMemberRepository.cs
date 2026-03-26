using System;
using Domain;

namespace Application.Interfaces;

public interface IMemberRepository
{
    void Add(OrganizationMember member);
    Task<OrganizationMember?> GetByOrgIdAsync(string id, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
