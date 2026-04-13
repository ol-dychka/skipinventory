namespace Domain;

public class SaleRecord
{
    // core
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Sku { get; set; }
    public DateTime Date { get; set; }

    // numbers
    public int UnitsSold { get; set; }
    public int UnitsReturned { get; set; }

    // relationships
    public required string OrganizationId { get; set; }
    public required Organization Organization { get; set; }
    public required string ProductId { get; set; }
    public required Product Product { get; set; }
}
