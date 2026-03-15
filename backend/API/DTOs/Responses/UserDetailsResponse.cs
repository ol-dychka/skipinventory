using System;
using Domain;

namespace API.DTOs.Responses;

public class UserDetailsResponse(User user)
{
    public string Id { get; set; } = user.Id;
    public Organization? Organization { get; set; } = user.Organization;
    public string Name { get; set; } = user.Name;
    public string Email { get; set; } = user.Email;
    public string Role { get; set; } = user.Role;
}
