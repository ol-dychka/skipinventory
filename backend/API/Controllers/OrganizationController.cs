using System;
using System.IdentityModel.Tokens.Jwt;
using API.DTOs.Requests.Organizations;
using API.DTOs.Responses;
using Application.Organizations.Commands;
using Application.Organizations.Queries;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class OrganizationController : BaseAPIController
{
    // refactor
    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        var result = await Mediator.Send(new List.Query());
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        var organizations = result.Value.Select(o => new OrganizationPreviewDto(o));

        return Ok(organizations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(string id)
    {
        var result = await Mediator.Send(new Details.Query(id));
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        var organizationDto = new OrganizationDto(result.Value);

        return Ok(organizationDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequest request)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (userId == null)
            return Unauthorized("token does not exist");

        var result = await Mediator.Send(new Create.Command(request.Name, userId));
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        return Ok(new { organizationId = result.Value });
    }

    [HttpPost("{organizationId}/request")]
    public async Task<IActionResult> RequestJoin(string organizationId)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (userId == null)
            return Unauthorized("token does not exist");

        var result = await Mediator.Send(new Request.Command(organizationId, userId));
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        return Ok(new { organizationId = result.Value });
    }
}
