using System.Text.Json;
using Direct4Me.Core.RouteHandler;
using Direct4Me.Repository.Entities;

namespace Direct4Me.Core.Handler;

public interface IRouteHandler
{
    RouteEntity GenerateMockRoute();
}

public class RouteHandler : IRouteHandler
{
    // This method generates a mock route with mock packages
    public RouteEntity GenerateMockRoute()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dataPath = Path.Combine(basePath, "Data", "complete_data.json");

        var route = new RouteEntity
        {
            Id = Guid.NewGuid().ToString(),
            CreatedOn = DateTime.Now,
            Postboxes = GeneratePostboxesFromCities(dataPath),
            EstimatedTravelTime = 120, // e.g., 120 minutes
            TotalDistance = 50 // e.g., 50 kilometers
        };

        return route;
    }

    private static List<PostboxEntity> GeneratePostboxesFromCities(string jsonPath)
    {
        var realWorldData = LoadRealWorldData(jsonPath);

        return realWorldData.Cities.Select(city => new PostboxEntity
            {
                Id = Guid.NewGuid().ToString(),
                PostBoxId = city.index,
                Latitude = city.cordX,
                Longitude = city.cordY,
                CreatedOn = DateTime.Now,
                StatisticsEntity = new PostboxStatisticsEntity() // Add additional details if needed
            })
            .ToList();
    }

    private static RealWorldData LoadRealWorldData(string jsonPath)
    {
        var json = File.ReadAllText(jsonPath);
        return JsonSerializer.Deserialize<RealWorldData>(json) ??
               throw new InvalidOperationException("Failed to load data from JSON.");
    }
}