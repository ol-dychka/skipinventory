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
    decimal costPrice,
    decimal salePrice,
    int currentStock,
    int reorderPoint,
    int baseReorderQuantity,
    int deliveryDelay,
    string? category
)
{
    // core
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = name;
    public string Sku { get; set; } = sku;
    public string? Category { get; set; } = category;
    public string Vendor { get; set; } = vendor;
    public ProductUnit Unit { get; set; } = ProductUnit.Each; //maybe change later

    // price
    public decimal CostPrice { get; set; } = costPrice;
    public decimal SalePrice { get; set; } = salePrice;
    public string Currency { get; set; } = "CAD";

    // inventory
    public int CurrentStock { get; set; } = currentStock;
    public int ReorderPoint { get; set; } = reorderPoint;
    public int BaseReorderQuantity { get; set; } = baseReorderQuantity;
    public int DeliveryDelay { get; set; } = deliveryDelay;

    // history
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // relationships
    public string OrganizationId { get; set; } = organizationId;
    public Organization Organization { get; set; } = null!;
}
