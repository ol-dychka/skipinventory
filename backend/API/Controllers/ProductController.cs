using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.DTOs.Requests.Products;
using API.DTOs.Responses;
using Application.Products.Commands;
using Application.Products.Queries;
using Domain.StaticClasses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductController : BaseAPIController
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
                Name = request.Name,
                Sku = request.Sku,
                Vendor = request.Vendor,
                OrganizationId = organizationId,
                CostPrice = request.CostPrice,
                SalePrice = request.SalePrice,
                CurrentStock = request.CurrentStock,
                ReorderPoint = request.ReorderPoint,
                BaseReorderQuantity = request.BaseReorderQuantity,
                DeliveryDelay = request.DeliveryDelay,
                Category = request.Category,
            }
        );
        if (!result.IsSuccess)
            return Unauthorized(result.Error);

        return NoContent();
    }

    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        var organizationId = User.FindFirst("org_id")?.Value;
        if (organizationId == null)
            return Unauthorized("current organization does not exist");

        var result = await Mediator.Send(new List.Query(organizationId));
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        var products = result.Value.Select(o => new ProductDto(o));

        return Ok(products);
    }
}
