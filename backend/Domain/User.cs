namespace Domain;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string OrganizationId { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Role { get; set; }
}