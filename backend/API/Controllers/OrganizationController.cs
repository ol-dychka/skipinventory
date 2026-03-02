using System;
using Application.Organizations.Commands;
using Application.Organizations.Queries;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class OrganizationController : BaseAPIController
{
    [HttpGet]
    public async Task<ActionResult<List<Organization>>> GetOrganizations()
    {
        return await Mediator.Send(new GetOrganizationList.Query());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Organization>> GetOrganization(string id)
    {
        return await Mediator.Send(new GetOrganization.Query(id));
    }

    public record CreateOrganizationRequest(string Name);
    [HttpPost]
    public async Task<ActionResult<string>> Create([FromBody] CreateOrganizationRequest request)
    {
        var id = await Mediator.Send(new CreateOrganization.Command(request.Name));
        return Ok(id);
    }

    [HttpPut]
    public async Task<ActionResult> EditOrganization(Organization organization)
    {
        await Mediator.Send(new EditOrganization.Command(organization));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrganization(string id)
    {
        await Mediator.Send(new DeleteOrganization.Command(id));
        return Ok();
    }
}
