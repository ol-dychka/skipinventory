using System;
using System.Security.Claims;
using API.DTOs.Requests.Auth;
using API.DTOs.Responses;
using API.Middleware;
using Application.Auth.Commands;
using Application.Interfaces;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthController(IRedisTokenService tokenService) : BaseAPIController
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await Mediator.Send(
            new Register.Command(request.Email, request.Password, request.Name)
        );
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        await tokenService.IssueTokenAsync(result.Value.UserId, result.Value.AccessToken);

        Response.Cookies.SetRefreshToken(result.Value.RefreshTokenData);
        return Ok(new { accessToken = result.Value.AccessToken });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await Mediator.Send(new Login.Command(request.Email, request.Password));
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        await tokenService.IssueTokenAsync(result.Value.UserId, result.Value.AccessToken);

        Response.Cookies.SetRefreshToken(result.Value.RefreshTokenData);
        return Ok(new { accessToken = result.Value.AccessToken });
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        if (string.IsNullOrEmpty(RefreshToken))
            return Unauthorized("Refresh cookie is invalid");

        var result = await Mediator.Send(new Refresh.Command(RefreshToken, request.OrganizationId));
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        await tokenService.IssueTokenAsync(result.Value.UserId, result.Value.AccessToken);

        Response.Cookies.SetRefreshToken(result.Value.RefreshTokenData);
        return Ok(new { accessToken = result.Value.AccessToken });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (string.IsNullOrEmpty(RefreshToken))
            return Unauthorized("Refresh cookie is invalid");

        var result = await Mediator.Send(new Logout.Command(RefreshToken));
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        await tokenService.RevokeTokenAsync(result.Value.UserId);

        Response.Cookies.DeleteRefreshToken();
        return NoContent();
    }
}
