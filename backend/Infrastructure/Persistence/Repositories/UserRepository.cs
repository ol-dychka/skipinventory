using System;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(PsqlDbContext context) : IUserRepository
{
    private readonly PsqlDbContext _context = context;

    public void Add(User user)
    {
        _context.Users.Add(user);
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _context
            .Users.Include(u => u.Memberships)
                .ThenInclude(m => m.Organization)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);
    }
}
