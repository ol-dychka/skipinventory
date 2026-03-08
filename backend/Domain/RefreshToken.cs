using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain;

public class RefreshToken
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string UserId { get; set; }
    public required string Hash { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; } = false;

    private RefreshToken() {}

    [SetsRequiredMembers]
    public RefreshToken(string userId, string hash, DateTime expiresAt)
    {
        UserId = userId;
        Hash = hash;
        ExpiresAt = expiresAt;
    }
}
