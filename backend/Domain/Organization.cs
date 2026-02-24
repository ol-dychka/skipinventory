namespace Domain;

public class Organization
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public required string SubscriptionTier { get; set; }

}