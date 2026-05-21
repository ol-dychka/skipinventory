using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.DTOs.Responses;
using Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController : BaseAPIController
{
    [HttpGet("details")]
    public async Task<IActionResult> Details()
    {
        if (UserId == null)
            return Unauthorized("token does not exist");

        var result = await Mediator.Send(new Details.Query(UserId));
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        var userDto = new UserDto(result.Value);

        return Ok(userDto);
    }
}
