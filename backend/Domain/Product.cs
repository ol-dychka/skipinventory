namespace Domain;

public class Product
{
    // core
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public required string Sku { get; set; }
    public required string Category { get; set; }
    public required string Vendor { get; set; }
    public required string Unit { get; set; }

    // price
    public int CostPrice { get; set; }
    public int SalePrice { get; set; }
    public string Currency { get; set; } = "CAD";

    // inventory
    public int CurrentStock { get; set; }
    public int ReorderPoint { get; set; }
    public int BaseReorderQuantity { get; set; }
    public int DeliveryDelay { get; set; }

    // history
    public bool IsDiscontinued { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }

    // relationships
    public required string OrganizationId { get; set; }
    public required Organization Organization { get; set; } = null!;
}
