using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.DTOs.Requests.SaleRecords;
using Application.SaleRecords.Commands;
using Application.SaleRecords.Queries;
using Domain.StaticClasses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class SaleRecordController : BaseAPIController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequest request)
    {
        if (UserId == null)
            return Unauthorized("token does not exist");

        if (Role == null || !UserRole.HasResolveJoinRights(Role))
            return Unauthorized("unsufficient rights");

        if (OrganizationId == null)
            return Unauthorized("organization does not exist");

        var result = await Mediator.Send(
            new Create.Command
            {
                UserId = UserId,
                OrganizationId = OrganizationId,
                Date = request.Date,
                Data =
                [
                    .. request.Data.Select(sale => new Create.CreateSaleRecord(
                        sale.Id,
                        sale.Quantity,
                        sale.Sku
                    )),
                ],
            }
        );
        if (!result.IsSuccess)
            return Unauthorized(result.Error);

        return NoContent();
    }

    [HttpGet("{date}")]
    public async Task<IActionResult> Exists(DateOnly date)
    {
        if (OrganizationId == null)
            return Unauthorized("organization does not exist");

        var result = await Mediator.Send(new Exists.Query(OrganizationId, date));
        if (!result.IsSuccess)
            return Unauthorized(result.Error);

        return Ok(result.Value);
    }
}
