using System;
using Domain;

namespace Application.Interfaces;

public interface IOrganizationRepository
{
    void Add(Organization organization);
    void Delete(Organization organization);
    Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken);
    Task<List<Organization>> GetAllAsync(CancellationToken cancellationToken);
    Task<Organization?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
