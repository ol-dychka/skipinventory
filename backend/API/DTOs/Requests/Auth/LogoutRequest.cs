using System;

namespace API.DTOs.Requests.Auth;

public class LogoutRequest
{
    public required string RefreshToken { get; set; }
}
