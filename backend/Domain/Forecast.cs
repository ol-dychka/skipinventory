namespace Domain;

public class Forecast
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string OrganizationId { get; set; }
    public required string ProductId { get; set; }
    public DateTime ForecastDate { get; set; }
    public int PredictedQuantity { get; set; }
    public required string ModelVersion { get; set; }
    public DateTime DateGenerated { get; set; }
}