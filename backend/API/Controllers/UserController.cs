using System;
using API.DTOs;
using Application.Users.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController : BaseAPIController
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await Mediator.Send(new RegisterUser.Command
            (request.Email, request.Password, request.Name, request.IsOwner));

        if (!result.IsSuccess || result.Value == null) return BadRequest(result.Error);

        return Ok(new AuthResponse{
            AccessToken = result.Value.AccessToken, RefreshToken = result.Value.RefreshToken
        });
    }
}
