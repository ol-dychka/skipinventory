using System;
using System.Runtime.CompilerServices;
using Application.Organizations.Commands;
using Application.Organizations.Queries;
using Domain;
using MediatR;
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
        return await Mediator.Send(new GetOrganization.Query{Id = id});
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateOrganization(Organization organization)
    {
        return await Mediator.Send(new CreateOrganization.Command{Organization = organization});
    }

    [HttpPut]
    public async Task<ActionResult> EditOrganization(Organization organization)
    {
        await Mediator.Send(new EditOrganization.Command{Organization = organization});
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrganization(string id)
    {
        await Mediator.Send(new DeleteOrganization.Command{Id = id});
        return Ok();
    }
}
