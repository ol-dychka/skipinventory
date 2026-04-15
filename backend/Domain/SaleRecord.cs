namespace Domain;

public class SaleRecord(
    int unitsSold,
    string sku,
    string organizationId,
    string productId,
    int unitsReturned
)
{
    // core
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Sku { get; set; } = sku;
    public DateTime Date { get; set; } = DateTime.UtcNow;

    // numbers
    public int UnitsSold { get; set; } = unitsSold;
    public int UnitsReturned { get; set; } = unitsReturned;

    // relationships
    public string OrganizationId { get; set; } = organizationId;
    public Organization Organization { get; set; } = null!;
    public string ProductId { get; set; } = productId;
    public Product Product { get; set; } = null!;
}
