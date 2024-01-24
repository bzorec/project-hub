using Direct4Me.Core.Handler;
using Direct4Me.Core.Runner;
using Xunit.Abstractions;

namespace Direct4Me.UnitTests;

public class ArffGenerator
{
    private const int NumPackageBoxes = 50;
    private const int NumInstances = 10_000;
    private readonly ITestOutputHelper _testOutputHelper;

    public ArffGenerator(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void G()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dataPath = Path.Combine(basePath, "Data", "packages.json");
        var runner = new JavaRunner();
        var handler = new RouteHandler();
        var route = handler.GenerateMockRoute();

        var packages = handler.GeneratePackagesForRoute(route, basePath, dataPath);
        var optimizedPackages = handler.OptimizePackages(runner, packages, basePath, dataPath);
        _testOutputHelper.WriteLine(optimizedPackages.ToString());
    }
    
    [Fact]
    public void Generator()
    {
        var rand = new Random();

        using var arffFile = new StreamWriter("delivery_data.arff");
        arffFile.WriteLine("@relation delivery_data");
        arffFile.WriteLine("@attribute boxId numeric"); // Attribute for box ID
        arffFile.WriteLine("@attribute numPeopleHome {0,1,2,3,4,5}");
        arffFile.WriteLine("@attribute paymentType {manual,prepaid}");
        arffFile.WriteLine("@attribute needSignature {yes,no}");
        arffFile.WriteLine("@attribute packageType {perishable,electronics,clothing,others}");
        arffFile.WriteLine("@attribute deliveryUrgency {standard,expedited,immediate}");
        arffFile.WriteLine("@attribute dayOfWeek {monday,tuesday,wednesday,thursday,friday,saturday,sunday}");
        arffFile.WriteLine("@attribute repeatDelivery {yes,no}");
        arffFile.WriteLine("@attribute postboxCapacity numeric");
        arffFile.WriteLine("@attribute packagePickup {yes,no}");

        arffFile.WriteLine("@data");

        for (var instance = 0; instance < NumInstances; ++instance)
        {
            var boxId = rand.Next(1, NumPackageBoxes + 1); // Random box ID
            var numPeopleHome = rand.Next(0, 6);
            var paymentType = rand.NextDouble() > 0.5 ? "manual" : "prepaid";
            var needSignature = rand.NextDouble() > 0.5 ? "yes" : "no";
            var packageType = GetRandomPackageType(rand);
            var deliveryUrgency = GetRandomDeliveryUrgency(rand);
            var dayOfWeek = GetRandomDayOfWeek(rand);
            var repeatDelivery = rand.NextDouble() > 0.5 ? "yes" : "no";
            var capacityUsed = CalculateCapacityUsed(packageType);

            arffFile.Write(
                $"{boxId},{numPeopleHome},{paymentType},{needSignature},{packageType},{deliveryUrgency},{dayOfWeek},{repeatDelivery},{capacityUsed},");
            arffFile.WriteLine(DiscretizePackagePickup(numPeopleHome, deliveryUrgency, repeatDelivery,
                capacityUsed));

            // Output statistics for each instance
            _testOutputHelper.WriteLine("Instance " + (instance + 1) + " recorded");
            _testOutputHelper.WriteLine("-------------------------------------------");
        }
    }

    private static string DiscretizePackagePickup(int numPeopleHome, string deliveryUrgency,
        string repeatDelivery, int totalCapacityUsed)
    {
        // Modify this logic as per your need for successful delivery
        if (numPeopleHome > 0 && deliveryUrgency != "immediate" && repeatDelivery == "no" &&
            totalCapacityUsed < 100) // Example capacity check
            return "yes";
        return "no";
    }

    private static string GetRandomPackageType(Random rand)
    {
        string[] types = { "perishable", "electronics", "clothing", "others" };
        return types[rand.Next(types.Length)];
    }

    private static string GetRandomDeliveryUrgency(Random rand)
    {
        string[] urgencies = { "standard", "expedited", "immediate" };
        return urgencies[rand.Next(urgencies.Length)];
    }

    private static string GetRandomDayOfWeek(Random rand)
    {
        string[] days = ["monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday"];
        return days[rand.Next(days.Length)];
    }

    private static int CalculateCapacityUsed(string packageType)
    {
        // Implement logic to determine capacity used based on package type
        return packageType switch
        {
            "perishable" => 5,
            "electronics" => 10,
            "clothing" => 3,
            _ => 2
        };
    }
}