using System;
using Domain;

namespace API.DTOs.Responses;

public record MemberDto(string Role, string UserId, string UserName, string UserEmail);

public record JoinRequestDto(string Id, string UserId, string UserName, string UserEmail);

public class OrganizationDto(Organization organization)
{
    public string Id { get; set; } = organization.Id;
    public string Name { get; set; } = organization.Name;
    public string SubscriptionTier { get; set; } = organization.SubscriptionTier;
    public List<MemberDto> Members { get; set; } =
    [
        .. organization.Members.Select(m => new MemberDto(
            m.Role,
            m.UserId,
            m.User.Name,
            m.User.Email
        )),
    ];
    public List<JoinRequestDto> JoinRequests { get; set; } =
    [
        .. organization.JoinRequests.Select(jr => new JoinRequestDto(
            jr.Id,
            jr.UserId,
            jr.User.Name,
            jr.User.Email
        )),
    ];
}
