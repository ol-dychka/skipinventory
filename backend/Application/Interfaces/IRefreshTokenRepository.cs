using System;
using Domain;

namespace Application.Interfaces;

public interface IRefreshTokenRepository
{
    public void Add(RefreshToken token);
    public void Delete(RefreshToken token);
    Task<RefreshToken?> GetByHashAsync(string hash, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
