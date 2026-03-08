using System;
using API.DTOs;
using Application.Users.Commands;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserControllerd : BaseAPIController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var tokens = await Mediator.Send(new RegisterUser.Command
            (request.Email, request.Password, request.OrganizationId, request.IsOwner));
        return Ok(tokens);
    }
}
