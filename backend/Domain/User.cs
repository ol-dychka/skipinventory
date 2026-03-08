using System.Diagnostics.CodeAnalysis;

namespace Domain;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Role { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = [];

    private User() {}

    [SetsRequiredMembers]
    private User(string email, string passwordHash, string organizationId, string role)
    {
        Email = email;
        PasswordHash = passwordHash;
        OrganizationId = organizationId;
        Role = role;
    }
    public static User CreateEmployee(string email, string passwordHash, string organizationId)
    {
        return new User(email, passwordHash, organizationId, UserRole.Employee);
    } 

    public static User CreateOwner(string email, string passwordHash, string organizationId)
    {
        return new User(email, passwordHash, organizationId, UserRole.Owner);
    } 
}