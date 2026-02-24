namespace Domain;

public class Product
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string OrganizationId { get; set; }
    public required string Name { get; set; }
    public required string Sku { get; set; }
    public int CurrentStock { get; set; }
    public int ReorderThreshold { get; set; }
    public bool IsDeleted { get; set; }
}