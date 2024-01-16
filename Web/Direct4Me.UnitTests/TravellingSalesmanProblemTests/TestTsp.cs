using Direct4Me.Core.TravellingSalesmanProblem;
using FluentAssertions;
using Xunit.Abstractions;

namespace Direct4Me.UnitTests.TravellingSalesmanProblemTests;

public class TestTsp
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TestTsp(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("bays29.tsp", 1000)]
    [InlineData("bays29.tsp", 10000)]
    [InlineData("bays29.tsp", 100000)]
    [InlineData("bays29.tsp", 1000000)]
    [InlineData("eil101.tsp", 1000)]
    [InlineData("eil101.tsp", 10000)]
    [InlineData("eil101.tsp", 100000)]
    [InlineData("eil101.tsp", 1000000)]
    [InlineData("a280.tsp", 1000)]
    [InlineData("a280.tsp", 10000)]
    [InlineData("a280.tsp", 100000)]
    [InlineData("a280.tsp", 1000000)]
    [InlineData("pr1002.tsp", 1000)]
    [InlineData("pr1002.tsp", 10000)]
    [InlineData("pr1002.tsp", 100000)]
    [InlineData("pr1002.tsp", 1000000)]
    [InlineData("dca1389.tsp", 1000)]
    [InlineData("dca1389.tsp", 10000)]
    [InlineData("dca1389.tsp", 100000)]
    [InlineData("dca1389.tsp", 1000000)]
    public void TestTspAlgorithm(string fileName, int maxEfs)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dataPath = Path.Combine(basePath, "Data", fileName);

        var bestBest = new Tour(0);
        var distances = new List<double>();

        for (var i = 0; i < 30; i++)
        {
            var eilTsp = new TspAlgorithm(dataPath, maxEfs);
            var ga = new GeneticAlgorithm(100, 0.8, 0.1);
            var bestPath = ga.Execute(eilTsp);

            // _testOutputHelper.WriteLine("{0}. [{1}]", i + 1, bestPath);
            distances.Add(bestPath.Distance);

            if (bestBest.Distance > bestPath.Distance)
            {
                bestBest = new Tour(bestPath);
            }

            bestPath.Should().NotBeNull();
            bestPath.Path.Count.Should().BeGreaterThan(0);
        }

        double avgDistance = distances.Average();
        double stdDistance = Math.Sqrt(distances.Sum(d => Math.Pow(d - avgDistance, 2)) / distances.Count);

        _testOutputHelper.WriteLine("Best: [{0}]", bestBest);
        _testOutputHelper.WriteLine("Avg: [{0}]", avgDistance);
        _testOutputHelper.WriteLine("Std: [{0}]", stdDistance);
    }

    [Theory]
    [InlineData("bays29.tsp", 1000)]
    [InlineData("bays29.tsp", 10000)]
    [InlineData("bays29.tsp", 100000)]
    [InlineData("bays29.tsp", 1000000)]
    [InlineData("eil101.tsp", 1000)]
    [InlineData("eil101.tsp", 10000)]
    [InlineData("eil101.tsp", 100000)]
    [InlineData("eil101.tsp", 1000000)]
    [InlineData("a280.tsp", 1000)]
    [InlineData("a280.tsp", 10000)]
    [InlineData("a280.tsp", 100000)]
    [InlineData("a280.tsp", 1000000)]
    [InlineData("pr1002.tsp", 1000)]
    [InlineData("pr1002.tsp", 10000)]
    [InlineData("pr1002.tsp", 100000)]
    [InlineData("pr1002.tsp", 1000000)]
    [InlineData("dca1389.tsp", 1000)]
    [InlineData("dca1389.tsp", 10000)]
    [InlineData("dca1389.tsp", 100000)]
    [InlineData("dca1389.tsp", 1000000)]
    public void TestTspAlgorithm1(string fileName, int maxEfs)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dataPath = Path.Combine(basePath, "Data", fileName);

        var bestBest = new Tour(0);
        var distances = new List<double>();

        for (var i = 0; i < 30; i++)
        {
            var eilTsp = new TspAlgorithm(dataPath, maxEfs);
            var ga = new GeneticAlgorithm(100, 0.8, 0.8);
            var bestPath = ga.Execute(eilTsp);

            // _testOutputHelper.WriteLine("{0}. [{1}]", i + 1, bestPath);
            distances.Add(bestPath.Distance);

            if (bestBest.Distance > bestPath.Distance)
            {
                bestBest = new Tour(bestPath);
            }

            bestPath.Should().NotBeNull();
            bestPath.Path.Count.Should().BeGreaterThan(0);
        }

        double avgDistance = distances.Average();
        double stdDistance = Math.Sqrt(distances.Sum(d => Math.Pow(d - avgDistance, 2)) / distances.Count);

        _testOutputHelper.WriteLine("Best: [{0}]", bestBest);
        _testOutputHelper.WriteLine("Avg: [{0}]", avgDistance);
        _testOutputHelper.WriteLine("Std: [{0}]", stdDistance);
    }

    [Fact]
    public void TestTspAlgorithmReal()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dataPath = Path.Combine(basePath, "Data", "complete_data.json");

        var bestBest = new Tour(0);
        var distances = new List<double>();

        for (var i = 0; i < 30; i++)
        {
            var eilTsp = new TspAlgorithm(dataPath, 1000, true, true);
            var ga = new GeneticAlgorithm(100, 0.8, 0.1);
            var bestPath = ga.Execute(eilTsp);

            _testOutputHelper.WriteLine("{0}. [{1}]", i + 1, bestPath);
            distances.Add(bestPath.Distance);

            if (bestBest.Distance > bestPath.Distance)
            {
                bestBest = new Tour(bestPath);
            }

            bestPath.Should().NotBeNull();
            bestPath.Path.Count.Should().BeGreaterThan(0);
        }

        double avgDistance = distances.Average();
        double stdDistance = Math.Sqrt(distances.Sum(d => Math.Pow(d - avgDistance, 2)) / distances.Count);

        _testOutputHelper.WriteLine("Best: [{0}]", bestBest);
        _testOutputHelper.WriteLine("Avg: [{0}]", avgDistance);
        _testOutputHelper.WriteLine("Std: [{0}]", stdDistance);
    }
}