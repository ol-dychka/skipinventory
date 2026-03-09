using System;
using API.DTOs.Requests.Auth;
using API.DTOs.Responses;
using Application.Auth.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthController : BaseAPIController
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await Mediator.Send(new Register.Command
            (request.Email, request.Password, request.Name, request.IsOwner));

        if (!result.IsSuccess || result.Value == null) return Unauthorized(result.Error);

        return Ok(new AuthResponse{
            AccessToken = result.Value.AccessToken, RefreshToken = result.Value.RefreshToken
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await Mediator.Send(new Login.Command(request.Email, request.Password));

        if (!result.IsSuccess || result.Value == null) return Unauthorized(result.Error);

        return Ok(new AuthResponse{
            AccessToken = result.Value.AccessToken, RefreshToken = result.Value.RefreshToken
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var result = await Mediator.Send(new Refresh.Command(request.RefreshToken));

        if (!result.IsSuccess || result.Value == null) return Unauthorized(result.Error);

        return Ok(new AuthResponse{
            AccessToken = result.Value.AccessToken, RefreshToken = result.Value.RefreshToken
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        var result = await Mediator.Send(new Logout.Command(request.RefreshToken));

        if (!result.IsSuccess) return Unauthorized(result.Error);

        return Ok();
    }
}
