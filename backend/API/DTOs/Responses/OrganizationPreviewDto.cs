using System;
using Domain;

namespace API.DTOs.Responses;

public class OrganizationPreviewDto(Organization organization)
{
    public string Id { get; set; } = organization.Id;
    public string Name { get; set; } = organization.Name;
}
