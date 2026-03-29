using System;
using Domain;

namespace API.DTOs.Responses;

public class OrganizationDto(Organization organization)
{
    public string Id = organization.Id;
    public string Name = organization.Name;
    public string SubscriptionTier = organization.SubscriptionTier;
    public UserDto Creator = new(organization.Creator);
}
