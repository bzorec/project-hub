using Direct4Me.Repository.Entities;

namespace Direct4Me.Core.RouteHandler;

public class RouteHandler
{
    // This method generates a mock route with mock packages
    public RouteEntity GenerateMockRoute()
    {
        var route = new RouteEntity
        {
            Id = Guid.NewGuid().ToString(),
            CreatedOn = DateTime.Now,
            Postboxes = GenerateMockPostboxes(),
            EstimatedTravelTime = 120, // e.g., 120 minutes
            TotalDistance = 50 // e.g., 50 kilometers
        };

        return route;
    }

    // This method generates a list of mock postboxes with mock packages
    private List<PostboxEntity> GenerateMockPostboxes()
    {
        var postboxes = new List<PostboxEntity>();
        var rand = new Random();

        // Generate mock postboxes
        for (var i = 0; i < 5; i++) // Assuming 5 postboxes for the mock route
        {
            var postbox = new PostboxEntity
            {
                Id = Guid.NewGuid().ToString(),
                PostBoxId = rand.Next(1, 1000),
                Latitude = GetRandomLatitude(),
                Longitude = GetRandomLongitude(),
                CreatedOn = DateTime.Now,
                StatisticsEntity = new PostboxStatisticsEntity() // Add additional details if needed
            };

            postboxes.Add(postbox);
        }

        return postboxes;
    }

    // Generates a random latitude - for demonstration purposes
    private double GetRandomLatitude()
    {
        var rand = new Random();
        return rand.NextDouble() * 180 - 90; // Latitude ranges from -90 to 90
    }

    // Generates a random longitude - for demonstration purposes
    private double GetRandomLongitude()
    {
        var rand = new Random();
        return rand.NextDouble() * 360 - 180; // Longitude ranges from -180 to 180
    }
}