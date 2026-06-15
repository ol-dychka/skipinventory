namespace Domain;

public class SaleForecast(
    string sku,
    string organizationId,
    string productId,
    DateOnly forecastStart,
    DateOnly forecastEnd,
    int predictedQuantity,
    bool suggestReorder,
    string modelVersion = "1",
    int lowerLimit = 0,
    int upperLimit = 0,
    int confidenceScore = 100
)
{
    // core
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Sku { get; set; } = sku;
    public string ModelVersion { get; set; } = modelVersion;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public DateOnly ForecastStart { get; set; } = forecastStart;
    public DateOnly ForecastEnd { get; set; } = forecastEnd;

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
