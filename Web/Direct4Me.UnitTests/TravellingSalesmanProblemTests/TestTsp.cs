using Direct4Me.Core.TravellingSalesmanProblem;

namespace Direct4Me.UnitTests.TravellingSalesmanProblemTests;

public class TestTsp
{
    private readonly Random _random = new(123);

    [Fact]
    public void TestTspAlgorithm()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dataPath = Path.Combine(basePath, "Data", "eil101.tsp");

        for (var i = 0; i < 100; i++)
        {
            var eilTsp = new TspAlgorithm(dataPath, 10000);
            var ga = new GeneticAlgorithm(100, 0.8, 0.1);
            var bestPath = ga.Execute(eilTsp);

            // Here you can assert specific conditions about bestPath
            // For example, you can check if the bestPath's distance is within expected range

            // Assert.True(bestPath.Distance < someExpectedMaximum);
            // Assert.True(bestPath.Distance > someExpectedMinimum);

            // Save min, avg, and std
        }
    }
}