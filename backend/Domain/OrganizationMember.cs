using System;

namespace Domain;

public class OrganizationMember(
    string userId,
    string organizationId,
    string role)
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = userId;
    public string OrganizationId { get; set; } = organizationId;
    public string Role { get; set; } = role;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    // relationships
    public User User { get; set; } = null!;
    public Organization Organization { get; set; } = null!;
}
