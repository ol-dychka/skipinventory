using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.DTOs.Responses;
using Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController : BaseAPIController
{
    [HttpPost("details")]
    public async Task<IActionResult> Register()
    {
        var id = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (id == null) return Unauthorized("token does not exist");

        var result = await Mediator.Send(new Details.Query(id));
        if (!result.IsSuccess || result.Value == null) return Unauthorized(result.Error);

        var userDTO = new UserDetailsResponse(result.Value);

        return Ok(new { user = userDTO });
    }
}
