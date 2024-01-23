using System.Text.Json.Serialization;
using Direct4Me.Core.TravellingSalesmanProblem;

namespace Direct4Me.Core.RouteHandler;

public class RealWorldData
{
    [JsonPropertyName("cities")] public List<City> Cities { get; set; } = new();

    [JsonPropertyName("distanceMatrix")] public double[,] DistanceMatrix { get; set; } = new double[0, 0];

    [JsonPropertyName("timeMatrix")] public double[,] TimeMatrix { get; set; } = new double[0, 0];
}