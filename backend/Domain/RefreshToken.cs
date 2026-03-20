using System;

namespace Domain;

public class RefreshToken(string userId, string hash, DateTime expiresAt)
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = userId;
    public string Hash { get; set; } = hash;
    public DateTime ExpiresAt { get; set; } = expiresAt;
    public bool IsRevoked { get; set; } = false;

    // relationships
    public User User { get; set; } = null!;
}
