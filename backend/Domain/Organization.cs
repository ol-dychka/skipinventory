namespace Domain;

public class Organization(string name, string creatorId)
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = name;
    public string CreatorId { get; set; } = creatorId;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
    public string SubscriptionTier { get; set; } = StaticClasses.SubscriptionTier.Free;

    // relationships
    public User Creator { get; set; } = null!;
    public List<OrganizationMember> Members { get; set; } = [];
    public List<JoinRequest> JoinRequests { get; set; } = [];
}