using Domain;

namespace API.DTOs.Responses;

public class SaleForecastDto(SaleForecast sf)
{
    public string Id { get; set; } = sf.Id;
    public string Sku { get; set; } = sf.Sku;
    public DateTime GeneratedAt { get; set; } = sf.GeneratedAt;
    public DateOnly ForecastStart { get; set; } = sf.ForecastStart;
    public DateOnly ForecastEnd { get; set; } = sf.ForecastEnd;

    // numbers
    public int PredictedQuantity { get; set; } = sf.PredictedQuantity;

    // ordering
    public bool SuggestReorder { get; set; } = sf.SuggestReorder;

    // relationships
    public string ProductId { get; set; } = sf.ProductId;
}
