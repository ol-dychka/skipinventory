namespace Domain;

public class SaleForecast(string sku, string modelVersion, string organizationId, string productId)
{
    // core
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Sku { get; set; } = sku;
    public string ModelVersion { get; set; } = modelVersion;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public DateTime ForecastStart { get; set; }
    public DateTime ForecastEnd { get; set; }

    // numbers
    public int PredictedQuantity { get; set; }
    public int LowerLimit { get; set; }
    public int UpperLimit { get; set; }
    public int ConfidenceScore { get; set; }

    // ordering
    public bool SuggestReorder { get; set; }

    // relationships
    public string OrganizationId { get; set; } = organizationId;
    public Organization Organization { get; set; } = null!;
    public string ProductId { get; set; } = productId;
    public Product Product { get; set; } = null!;
}
