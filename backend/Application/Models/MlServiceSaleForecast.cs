using System.Text.Json.Serialization;

namespace Application.Models
{
    public class MlServiceSaleForecast
    {
        [JsonPropertyName("sku")]
        public string Sku { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("predicted_weekly_demand")]
        public double PredictedWeeklyDemand { get; set; }

        [JsonPropertyName("recommended_order_quantity")]
        public int RecommendedOrderQuantity { get; set; }
    }
}
