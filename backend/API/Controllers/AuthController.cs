using System;
using API.DTOs.Requests.Auth;
using API.DTOs.Responses;
using API.Middleware;
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

        Response.Cookies.SetRefreshToken(result.Value.RefreshTokenData);
        return Ok(new {
            accessToken = result.Value.AccessToken
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await Mediator.Send(new Login.Command(request.Email, request.Password));
        if (!result.IsSuccess || result.Value == null) return Unauthorized(result.Error);

        Response.Cookies.SetRefreshToken(result.Value.RefreshTokenData);
        return Ok(new {
            accessToken = result.Value.AccessToken
        });
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken)) return Unauthorized("Refresh cookie is invalid");

        var result = await Mediator.Send(new Refresh.Command(refreshToken));
        if (!result.IsSuccess || result.Value == null) return Unauthorized(result.Error);

        Response.Cookies.SetRefreshToken(result.Value.RefreshTokenData);
        return Ok(new {
            accessToken = result.Value.AccessToken
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken)) return Unauthorized("Refresh cookie is invalid");

        var result = await Mediator.Send(new Logout.Command(refreshToken));
        if (!result.IsSuccess) return Unauthorized(result.Error);

        Response.Cookies.DeleteRefreshToken();
        return NoContent();
    }
}
