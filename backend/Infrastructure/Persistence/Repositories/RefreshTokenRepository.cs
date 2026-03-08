using System;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(PsqlDbContext context) : IRefreshTokenRepository
{
    private readonly PsqlDbContext _context = context;

    public void Add(RefreshToken token)
    {
        _context.RefreshTokens.Add(token);
    }

    public void Delete(RefreshToken token)
    {
        _context.RefreshTokens.Remove(token);
    }

    public async Task<RefreshToken?> GetByHashAsync(string hash, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Hash == hash, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
