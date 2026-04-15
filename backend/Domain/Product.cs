namespace Domain;

public enum ProductUnit
{
    Each,
    Kg,
    Box,
    Litre,
}

public class Product(
    string name,
    string sku,
    string vendor,
    string organizationId,
    string? category
)
{
    // core
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = name;
    public string Sku { get; set; } = sku;
    public string? Category { get; set; } = category;
    public string Vendor { get; set; } = vendor;
    public ProductUnit Unit { get; set; }

    // price
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public string Currency { get; set; } = "CAD";

    // inventory
    public int CurrentStock { get; set; }
    public int ReorderPoint { get; set; }
    public int BaseReorderQuantity { get; set; }
    public int DeliveryDelay { get; set; }

    // history
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // relationships
    public string OrganizationId { get; set; } = organizationId;
    public Organization Organization { get; set; } = null!;
}
