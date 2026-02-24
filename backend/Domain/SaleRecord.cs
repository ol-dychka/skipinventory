namespace Domain;

public class SaleRecord
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string OrganizationId { get; set; }
    public required string ProductId { get; set; }
    public DateTime SaleDate { get; set; }
    public int QuantitySold { get; set; }
}