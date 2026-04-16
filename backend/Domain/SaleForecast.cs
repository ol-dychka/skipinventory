namespace Domain;

public class SaleForecast(
    string sku,
    string modelVersion,
    string organizationId,
    string productId,
    DateTime forecastStart,
    DateTime forecastEnd,
    int predictedQuantity,
    int lowerLimit,
    int upperLimit,
    int confidenceScore,
    bool suggestReorder
)
{
    // core
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Sku { get; set; } = sku;
    public string ModelVersion { get; set; } = modelVersion;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public DateTime ForecastStart { get; set; } = forecastStart;
    public DateTime ForecastEnd { get; set; } = forecastEnd;

    // numbers
    public int PredictedQuantity { get; set; } = predictedQuantity;
    public int LowerLimit { get; set; } = lowerLimit;
    public int UpperLimit { get; set; } = upperLimit;
    public int ConfidenceScore { get; set; } = confidenceScore;

    // ordering
    public bool SuggestReorder { get; set; } = suggestReorder;

    // relationships
    public string OrganizationId { get; set; } = organizationId;
    public Organization Organization { get; set; } = null!;
    public string ProductId { get; set; } = productId;
    public Product Product { get; set; } = null!;
}
