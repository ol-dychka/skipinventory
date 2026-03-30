using System;
using Domain;

namespace API.DTOs.Responses;

public class OrganizationDto(Organization organization)
{
    public string Id { get; set; } = organization.Id;
    public string Name { get; set; } = organization.Name;
    public string SubscriptionTier { get; set; } = organization.SubscriptionTier;
    // public UserDto Creator { get; set; } = new(organization.Creator);
}
