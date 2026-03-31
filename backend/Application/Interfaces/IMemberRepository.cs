using System;
using Domain;

namespace Application.Interfaces;

public interface IMemberRepository
{
    void Add(OrganizationMember member);
    Task<OrganizationMember?> GetAsync(
        string organizationId,
        string userId,
        CancellationToken cancellationToken
    );
}
