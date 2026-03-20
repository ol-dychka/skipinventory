using System;
using Domain;

namespace API.DTOs.Responses;

public record MembershipDto
(
    string Role,
    string OrganizationId,
    string OrganizationName
);

public class UserDto(User user)
{
    public string Id { get; set; } = user.Id;
    public string Name { get; set; } = user.Name;
    public string Email { get; set; } = user.Email;
    public List<MembershipDto> Memberships { get; set; } =
        [.. user.Memberships.Select(m =>
            new MembershipDto(m.Role, m.OrganizationId, m.Organization.Name))];
}
