using System;
using Domain;

namespace Application.Interfaces;

public interface IUserRepository
{
    void Add(User user);
    void Delete(User user);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByIdWithOrganizationAsync(string id, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
