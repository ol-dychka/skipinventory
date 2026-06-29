using System;
using Domain;

namespace API.DTOs.MlService;

public class ProductDto(Product product)
{
    public string Id { get; set; } = product.Id;
    public string Name { get; set; } = product.Name;
    public string Sku { get; set; } = product.Sku;
    public string? Category { get; set; } = product.Category;
    public string Vendor { get; set; } = product.Vendor;
    public string Unit { get; set; } = product.Unit.ToString();
    public string OrganizationId { get; set; } = product.OrganizationId;

    // price
    public decimal CostPrice { get; set; } = product.CostPrice;
    public decimal SalePrice { get; set; } = product.SalePrice;

    // inventory
    public int CurrentStock { get; set; } = product.CurrentStock;
    public int ReorderPoint { get; set; } = product.ReorderPoint;
    public int BaseReorderQuantity { get; set; } = product.BaseReorderQuantity;
    public int DeliveryDelay { get; set; } = product.DeliveryDelay;
}
