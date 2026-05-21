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
                Name = request.Name,
                Sku = request.Sku,
                Vendor = request.Vendor,
                OrganizationId = OrganizationId,
                CostPrice = request.CostPrice,
                SalePrice = request.SalePrice,
                CurrentStock = request.CurrentStock,
                ReorderPoint = request.ReorderPoint,
                BaseReorderQuantity = request.BaseReorderQuantity,
                DeliveryDelay = request.DeliveryDelay,
                Category = request.Category,
            }
        );
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        var productDto = new ProductDto(result.Value);
        return Ok(productDto);
    }

    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        if (OrganizationId == null)
            return Unauthorized("current organization does not exist");

        var result = await Mediator.Send(new List.Query(OrganizationId));
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        var products = result.Value.Select(o => new ProductDto(o));

        return Ok(products);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit([FromBody] EditRequest request, string id)
    {
        if (UserId == null)
            return Unauthorized("token does not exist");

        if (Role == null || !UserRole.HasResolveJoinRights(Role))
            return Unauthorized("unsufficient rights");

        if (OrganizationId == null)
            return Unauthorized("organization does not exist");

        var result = await Mediator.Send(
            new Edit.Command
            {
                UserId = UserId,
                ProductId = id,
                Name = request.Name,
                Sku = request.Sku,
                Vendor = request.Vendor,
                OrganizationId = OrganizationId,
                CostPrice = request.CostPrice,
                SalePrice = request.SalePrice,
                CurrentStock = request.CurrentStock,
                ReorderPoint = request.ReorderPoint,
                BaseReorderQuantity = request.BaseReorderQuantity,
                DeliveryDelay = request.DeliveryDelay,
                IsActive = request.IsActive,
                Category = request.Category,
            }
        );
        if (!result.IsSuccess || result.Value == null)
            return Unauthorized(result.Error);

        var productDto = new ProductDto(result.Value);
        return Ok(productDto);
    }
}
