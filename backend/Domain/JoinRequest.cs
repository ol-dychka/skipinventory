using System;
using Domain.StaticClasses;

namespace Domain;

public class JoinRequest(string userId, string organizationId)
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = userId;
    public string OrganizationId { get; set; } = organizationId;
    public string Status { get; set; } = JoinRequestStatus.Pending;
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }
    public string? ResolverId { get; set; }

    // relationships
    public User User { get; set; } = null!;
    public Organization Organization { get; set; } = null!;
    public User? Resolver { get; set; }
}
