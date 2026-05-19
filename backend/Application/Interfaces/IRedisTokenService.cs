namespace Application.Interfaces;

public interface IRedisTokenService
{
    Task IssueTokenAsync(string userId, string token);
    Task<bool> VerifyTokenAsync(string userId, string incomingToken);
    Task RevokeTokenAsync(string userId);
}
