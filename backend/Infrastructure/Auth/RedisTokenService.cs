using Application.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Infrastructure.Auth;

public class RedisTokenService(IConnectionMultiplexer redis, IOptions<JwtSettings> jwtOptions)
    : IRedisTokenService
{
    private readonly IDatabase _redis = redis.GetDatabase();
    private readonly TimeSpan _tokenTtl = TimeSpan.FromMinutes(
        jwtOptions.Value.AccessTokenExpiryMinutes
    );

    public async Task IssueTokenAsync(string userId, string token)
    {
        await _redis.StringSetAsync($"auth:token:{userId}", token, _tokenTtl);
    }

    public async Task<bool> VerifyTokenAsync(string userId, string incomingToken)
    {
        RedisValue latestToken = await _redis.StringGetAsync($"auth:token:{userId}");

        if (latestToken.IsNullOrEmpty)
            return false; // expired or never issued

        return latestToken == incomingToken;
    }

    public async Task RevokeTokenAsync(string userId)
    {
        await _redis.KeyDeleteAsync($"auth:token:{userId}");
    }
}
