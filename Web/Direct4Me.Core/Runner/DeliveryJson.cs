using System.Text.Json.Serialization;

namespace Direct4Me.Core.Runner;

public class DeliveryEstimateJson
{
    [JsonPropertyName("MAKINE")] public int MAKINE { get; set; }
    [JsonPropertyName("dayOfWeek")] public string DayOfWeek { get; set; }
    [JsonPropertyName("output")] public string Output { get; set; }
    [JsonPropertyName("delivery")] public List<int> Delivery { get; set; }
}