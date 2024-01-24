using System.Text.Json;
using System.Text.Json.Serialization;
using Direct4Me.Core.Runner;
using Direct4Me.Core.TravellingSalesmanProblem;
using Direct4Me.Repository.Entities;

namespace Direct4Me.Core.Handler;

public interface IRouteHandler
{
    RouteEntity GenerateMockRoute();
    List<PackageEntity> GeneratePackagesForRoute(RouteEntity route, string basePath, string jsonFilePath);

    List<PackageEntity> OptimizePackages(IJavaRunner javaRunner, List<PackageEntity> packages,
        string basePath, string dataPath);

    List<EstimetDelivery> GetEstimetDelivery(IJavaRunner javaRunner, Tour? aiOptimizedTour,
        string deliveryJsonPath);
}

public class EstimetDelivery
{
    public int PostBoxId { get; set; }
    public int EstimatedDeliveryTime { get; set; }
}

public class DeliveryJsonModel
{
    public Dictionary<int, int> postboxTimeNeededPairs { get; set; }
}

public class RouteHandler : IRouteHandler
{
    // This method generates a mock route with mock packages
    public RouteEntity GenerateMockRoute()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dataPath = Path.Combine(basePath, "Data", "complete_data.json");

        var (d1, d2) = GeneratePostboxesFromCities(dataPath);

        var route = new RouteEntity
        {
            Id = Guid.NewGuid().ToString(),
            CreatedOn = DateTime.Now,
            Postboxes = d1,
            DistanceMatrix = d2,
            EstimatedTravelTime = 120, // Example value
            TotalDistance = 50, // Example value
        };

        return route;
    }

    public List<PackageEntity> GeneratePackagesForRoute(RouteEntity route, string basePath, string jsonFilePath)
    {
        var packages = new List<PackageEntity>();
        var rand = new Random();

        foreach (var postbox in route.Postboxes)
        {
            int packageCount = rand.Next(2, 6); // 2 to 5 packages per postbox

            for (int i = 0; i < packageCount; i++)
            {
                packages.Add(new PackageEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    PackageId = rand.Next(1000, 9999),
                    PostBoxId = postbox.PostBoxId, // Use the actual postbox ID from the route
                    PackageType = GetRandomPackageType(rand),
                    Weight = rand.Next(1, 10), // Random weight from 1 to 10
                    DeliveryUrgency = GetRandomDeliveryUrgency(rand),
                    DayOfWeek = GetRandomDayOfWeek(rand),
                    PaymentType = GetRandomPaymentType(rand),
                    NeedSignature = GetRandomNeedSignature(rand),
                    NumPeopleHome = rand.Next(1, 6), // Random numPeopleHome from 1 to 5
                    PostboxCapacity = rand.Next(10, 100), // Random postboxCapacity from 10 to 100
                    RepeatDelivery = GetRandomRepeatDelivery(rand),
                    CreatedOn = DateTime.Now
                });
            }
        }

        // Serialize to JSON and write to file
        SerializeToJson(packages, jsonFilePath, basePath);

        return packages;
    }


    public void SerializeToJson(List<PackageEntity> packages, string jsonFilePath, string basePath)
    {
        var packageDataWrapper = new PackageDataWrapper
        {
            MAKINE = 1,
            Output = Path.Combine(basePath, "Data", "packages_output.json"),
            Delivery = packages
        };

        var jsonString =
            JsonSerializer.Serialize(packageDataWrapper, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(jsonFilePath, jsonString);
    }

    private string GetRandomDayOfWeek(Random rand)
    {
        var days = new[] { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };
        return days[rand.Next(days.Length)];
    }

    public class PackageDataWrapper
    {
        [JsonPropertyName("MAKINE")] public int MAKINE { get; set; }
        [JsonPropertyName("output")] public string Output { get; set; }
        [JsonPropertyName("delivery")] public List<PackageEntity> Delivery { get; set; }
    }

    private string GetRandomPaymentType(Random rand)
    {
        var types = new[] { "manual", "prepaid" };
        return types[rand.Next(types.Length)];
    }

    private string GetRandomNeedSignature(Random rand)
    {
        return rand.NextDouble() > 0.5 ? "yes" : "no";
    }

    private string GetRandomRepeatDelivery(Random rand)
    {
        return rand.NextDouble() > 0.5 ? "yes" : "no";
    }

    private string GetRandomPackageType(Random rand)
    {
        string[] types = { "perishable", "electronics", "clothing", "others" };
        return types[rand.Next(types.Length)];
    }

    private string GetRandomDeliveryUrgency(Random rand)
    {
        string[] urgencies = { "standard", "expedited", "immediate" };
        return urgencies[rand.Next(urgencies.Length)];
    }

    public List<PackageEntity> OptimizePackages(IJavaRunner javaRunner, List<PackageEntity> packages, string basePath,
        string dataPath)
    {
        // Run the Java program to get package pickup decisions
        javaRunner.RunJarAsync(dataPath);
        var rand = new Random();
        // Deserialize the decisions from the updated JSON file
        var packageDecisions = DeserializePackagesFromJson(Path.Combine(basePath, "Data", "packages_output.json"));

        foreach (var decision in packageDecisions)
        {
            decision.PickupDecision = rand.NextDouble() > 0.5 ? "yes" : "no";
            decision.Confidence = rand.NextDouble(); // Random confidence between 0.0 and 1.0
        }

        // Filter out packages based on the decision and confidence level
        var packageIdsToRemove = packageDecisions
            .Where(decision => decision.PickupDecision.Equals("no", StringComparison.OrdinalIgnoreCase) &&
                               decision.Confidence > 0.50)
            .Select(decision => decision.PackageId)
            .ToHashSet();

        // Keep only those packages whose IDs are not in the removal list
        var optimizedPackages = packages
            .Where(package => !packageIdsToRemove.Contains(package.PackageId))
            .OrderBy(p => p.DeliveryUrgency)
            .ToList();

        return optimizedPackages;
    }


    public List<EstimetDelivery> GetEstimetDelivery(IJavaRunner javaRunner, Tour? aiOptimizedTour,
        string deliveryJsonPath)
    {
        // Run the Java program to get delivery estimations
        javaRunner.RunJarAsync(deliveryJsonPath);

        // Load the delivery time estimates from the JSON file
        var deliveryTimes = DeserializeDeliveryTimes(deliveryJsonPath);

        // Create a list of EstimetDelivery based on aiOptimizedTour and deliveryTimes
        var estimetDeliveries = new List<EstimetDelivery>();
        if (aiOptimizedTour != null)
        {
            foreach (var city in aiOptimizedTour.Path)
            {
                if (city != null && deliveryTimes.postboxTimeNeededPairs.TryGetValue(city.index, out int deliveryTime))
                {
                    estimetDeliveries.Add(new EstimetDelivery
                    {
                        PostBoxId = city.index,
                        EstimatedDeliveryTime = deliveryTime // Assuming this is the desired format
                    });
                }
            }
        }

        return estimetDeliveries;
    }

    private DeliveryJsonModel DeserializeDeliveryTimes(string jsonFilePath)
    {
        var jsonString = File.ReadAllText(jsonFilePath);
        return System.Text.Json.JsonSerializer.Deserialize<DeliveryJsonModel>(jsonString) ??
               throw new InvalidOperationException();
    }


    private static (List<PostboxEntity>, List<List<int>>) GeneratePostboxesFromCities(string jsonPath)
    {
        var realWorldData = LoadRealWorldData(jsonPath);

        return (realWorldData.Cities.Select(city => new PostboxEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    PostBoxId = city.index,
                    Latitude = city.cordX,
                    Longitude = city.cordY,
                    CreatedOn = DateTime.Now,
                    StatisticsEntity = new PostboxStatisticsEntity() // Add additional details if needed
                })
                .Take(10)
                .ToList(),
            realWorldData.DistanceMatrix.Take(10).ToList());
    }

    private List<PackageDecision> DeserializePackagesFromJson(string jsonFilePath)
    {
        var jsonString = File.ReadAllText(jsonFilePath);
        return System.Text.Json.JsonSerializer.Deserialize<List<PackageDecision>>(jsonString) ??
               new List<PackageDecision>();
    }

    private static RealWorldData LoadRealWorldData(string jsonPath)
    {
        var json = File.ReadAllText(jsonPath);
        return JsonSerializer.Deserialize<RealWorldData>(json) ??
               throw new InvalidOperationException("Failed to load data from JSON.");
    }
}

public class PackageDecision
{
    public int PackageId { get; set; }
    public string PickupDecision { get; set; } // "yes" or "no"
    public double Confidence { get; set; } // Confidence level
}