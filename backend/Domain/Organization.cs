using System.Collections.Frozen;

namespace Domain;

public class Organization(string name)
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = name;
    public string SubscriptionTier { get; set; } = Domain.SubscriptionTier.Free;
}