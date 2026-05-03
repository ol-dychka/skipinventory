using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.DTOs.Requests.SaleRecords;
using Application.SaleRecords.Commands;
using Domain.StaticClasses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class SaleRecordController : BaseAPIController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequest request)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (userId == null)
            return Unauthorized("token does not exist");

        var role = User.FindFirstValue(ClaimTypes.Role);
        if (role == null || !UserRole.HasResolveJoinRights(role))
            return Unauthorized("unsufficient rights");

        var organizationId = User.FindFirst("org_id")?.Value;
        if (organizationId == null)
            return Unauthorized("organization does not exist");

        var result = await Mediator.Send(
            new Create.Command
            {
                UserId = userId,
                OrganizationId = organizationId,
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
}
