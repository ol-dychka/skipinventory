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
    [HttpGet]
    public async Task<ActionResult<List<Organization>>> GetOrganizations()
    {
        return await Mediator.Send(new GetOrganizationList.Query());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Organization>> GetOrganization(string id)
    {
        var result = await Mediator.Send(new Details.Query(id));
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        var organizationDto = new OrganizationDto(result.Value);

        return Ok(organizationDto);
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create([FromBody] CreateRequest request)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (userId == null)
            return Unauthorized("token does not exist");

        var result = await Mediator.Send(new Create.Command(request.Name, userId));
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        return Ok(new { organizationId = result.Value });
    }
}
