using System;
using Domain;

namespace Application.Interfaces;

public interface IOrganizationRepository
{
    void Add(Organization organization);
    void Delete(Organization organization);
    Task<List<Organization>> GetAllAsync(CancellationToken cancellationToken);
    Task<Organization?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
