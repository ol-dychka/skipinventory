using System;
using Domain;

namespace Application.Interfaces;

public interface IOrganizationRepository
{
    Task Add(Organization organization);
    Task Delete(Organization organization);
    Task<List<Organization>> GetAllAsync(CancellationToken cancellationToken);
    Task<Organization?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
