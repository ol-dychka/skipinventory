using System;

namespace API.DTOs;

public class RegisterRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string OrganizationId { get; set; }
    public bool IsOwner { get; set; }
}
