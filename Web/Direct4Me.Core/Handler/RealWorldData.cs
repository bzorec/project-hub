using System.Text.Json.Serialization;
using Direct4Me.Core.TravellingSalesmanProblem;

namespace Direct4Me.Core.Handler;

public class RealWorldData
{
    [JsonPropertyName("cities")] public List<City> Cities { get; set; }
    [JsonPropertyName("distanceMatrix")] public List<List<int>> DistanceMatrix { get; set; }
    [JsonPropertyName("timeMatrix")] public List<List<int>> TimeMatrix { get; set; }
}