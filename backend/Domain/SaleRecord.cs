namespace Domain;

public class SaleRecord(
    int unitsSold,
    string sku,
    string organizationId,
    string productId,
    int unitsReturned,
    DateOnly date
)
{
    // core
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Sku { get; set; } = sku;
    public DateOnly Date { get; set; } = date;

    // numbers
    public int UnitsSold { get; set; } = unitsSold;
    public int UnitsReturned { get; set; } = unitsReturned;

    // relationships
    public string OrganizationId { get; set; } = organizationId;
    public Organization Organization { get; set; } = null!;
    public string ProductId { get; set; } = productId;
    public Product Product { get; set; } = null!;
}
