using System;

namespace API.DTOs.Requests.Products;

public class EditRequest
{
    public required string Name { get; set; }
    public required string Sku { get; set; }
    public required string Vendor { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public int CurrentStock { get; set; }
    public int ReorderPoint { get; set; }
    public int BaseReorderQuantity { get; set; }
    public int DeliveryDelay { get; set; }
    public string? Category { get; set; }
    public bool IsActive { get; set; }
}
