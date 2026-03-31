namespace Domain;

public class User(string email, string passwordHash, string name)
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string PasswordHash { get; set; } = passwordHash;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // relationships
    public List<RefreshToken> RefreshTokens { get; set; } = [];
    public List<OrganizationMember> Memberships { get; set; } = [];
    public List<JoinRequest> PendingRequests { get; set; } = [];
    public List<JoinRequest> ResolvedRequests { get; set; } = [];
}
