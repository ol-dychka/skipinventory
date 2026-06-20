using Domain;

namespace API.DTOs.Responses;

public class SaleForecastDto(SaleForecast sf)
{
    public string Id { get; set; } = sf.Id;
    public string Sku { get; set; } = sf.Sku;
    public DateTime GeneratedAt { get; set; } = sf.GeneratedAt;
    public DateTime ForecastStart { get; set; } = sf.ForecastStart;
    public DateTime ForecastEnd { get; set; } = sf.ForecastEnd;

    // numbers
    public int PredictedQuantity { get; set; } = sf.PredictedQuantity;

    // relationships
    public string ProductId { get; set; } = sf.ProductId;
}
