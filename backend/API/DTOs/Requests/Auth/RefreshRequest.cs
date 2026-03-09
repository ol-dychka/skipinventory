using System;

namespace API.DTOs.Requests.Auth;

public class RefreshRequest
{
    public required string RefreshToken { get; set; }
}
