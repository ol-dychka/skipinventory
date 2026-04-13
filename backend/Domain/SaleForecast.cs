namespace Domain;

public class SaleForecast
{
    // core
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Sku { get; set; }
    public required string ModelVersion { get; set; }
    public DateTime GeneratedAt { get; set; }
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
    public required string OrganizationId { get; set; }
    public required Organization Organization { get; set; }
    public required string ProductId { get; set; }
    public required Product Product { get; set; }
}
