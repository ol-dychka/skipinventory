using System;

namespace API.DTOs.Responses;

public class AuthResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
