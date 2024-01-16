using System.Text.Json;

namespace Direct4Me.Core.TravellingSalesmanProblem;

public class TspAlgorithm
{
    private readonly Random _random;
    public List<City> Cities { get; private set; }
    public DistanceType DistanceType { get; private set; } = DistanceType.Euclidean;
    public int MaxEvaluations { get; private set; }
    public string? Name { get; private set; }
    public int NumberOfCities => Cities.Count;
    public int NumberOfEvaluations { get; private set; }
    public City? Start => Cities.FirstOrDefault(); // Assuming the start is always the first city
    public int[][]? Weights { get; private set; }

    public bool IsRealWorldData { get; private set; }
    public List<List<int>> DistanceMatrix { get; private set; }
    public List<List<int>> TimeMatrix { get; private set; }
    public bool OptimizeForTime { get; set; }

    public TspAlgorithm(string path, int maxEvaluations, bool isRealWorldData = false, bool optimizeForTime = false)
    {
        _random = new Random();
        MaxEvaluations = maxEvaluations;
        IsRealWorldData = isRealWorldData;
        OptimizeForTime = optimizeForTime;
        Cities = new List<City>();

        if (IsRealWorldData)
        {
            LoadRealWorldData(path);
        }
        else
        {
            LoadData(path);
        }
    }

    private void LoadRealWorldData(string jsonPath)
    {
        var json = File.ReadAllText(jsonPath);
        var data = JsonSerializer.Deserialize<RealWorldData>(json);

        Cities = data?.cities ?? throw new InvalidOperationException("Failed to load data from JSON.");
        DistanceMatrix = data.distanceMatrix;
        TimeMatrix = data.timeMatrix;
    }

    private void LoadData(string path)
    {
        Cities = new List<City>();

        var lines = File.ReadAllLines(path);

        if (lines.Contains("NODE_COORD_SECTION"))
        {
            DistanceType = DistanceType.Euclidean;
            ParseEuclideanData(lines);
        }
        else
        {
            DistanceType = DistanceType.Weighted;
            ParseExplicitWeightData(lines);
        }
    }

    private void ParseEuclideanData(string[] lines)
    {
        Cities = new List<City>();

        foreach (var line in lines)
        {
            if (line.StartsWith("NODE_COORD_SECTION"))
            {
                var cityLines = lines.SkipWhile(l => !l.StartsWith("NODE_COORD_SECTION")).Skip(1);
                foreach (var cityLine in cityLines)
                {
                    if (cityLine == "EOF") break;

                    var parts = cityLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var city = new City
                    {
                        index = int.Parse(parts[0]),
                        cordX = double.Parse(parts[1]),
                        cordY = double.Parse(parts[2])
                    };
                    Cities.Add(city);
                }

                break;
            }
        }
    }

    private void ParseExplicitWeightData(string[] lines)
    {
        Cities = new List<City>();
        Weights = Array.Empty<int[]>();

        foreach (var line in lines)
        {
            if (line.StartsWith("DIMENSION:"))
            {
                var dimension = int.Parse(line.Split(':')[1].Trim());
                Weights = new int[dimension][];
                continue;
            }

            if (line.StartsWith("EDGE_WEIGHT_SECTION"))
            {
                var weights = lines.SkipWhile(l => !l.StartsWith("EDGE_WEIGHT_SECTION")).Skip(1);
                var currentLineIndex = 0;
                foreach (var weight in weights)
                {
                    if (weight == "DISPLAY_DATA_SECTION") break;

                    var matrixLine = weight.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray();

                    Weights[currentLineIndex] = matrixLine;
                    currentLineIndex++;
                }
            }

            if (line.StartsWith("DISPLAY_DATA_SECTION"))
            {
                var cityLines = lines.SkipWhile(l => !l.StartsWith("DISPLAY_DATA_SECTION")).Skip(1);
                foreach (var cityLine in cityLines)
                {
                    if (cityLine == "EOF") break;

                    var parts = cityLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var city = new City
                    {
                        index = int.Parse(parts[0]),
                        cordX = double.Parse(parts[1]),
                        cordY = double.Parse(parts[2])
                    };
                    Cities.Add(city);
                }
            }
        }
    }

    public void Evaluate(Tour tour)
    {
        try
        {
            if (!IsRealWorldData)
            {
                if (tour.Path == null || tour.Path.Count != NumberOfCities)
                {
                    throw new InvalidOperationException(
                        "Tour path is not properly initialized or does not cover all cities.");
                }

                double distance = 0;

                // Calculate distance for each leg of the journey
                for (var index = 0; index < tour.Path.Count - 1; index++)
                {
                    distance += CalculateDistance(tour.Path[index], tour.Path[index + 1]);
                }

                // Add distance from the last city back to the start city
                distance += CalculateDistance(tour.Path.Last(), Start);

                tour.Distance = distance; // Or use another property if you want to store time separately
                NumberOfEvaluations++;
            }
            else
            {
                if (tour.Path == null || tour.Path.Count != NumberOfCities)
                    throw new InvalidOperationException("Tour path is not properly initialized or does not cover all cities.");

                double totalDistance = 0;
                double totalTime = 0;

                for (int i = 0; i < tour.Path.Count - 1; i++)
                {
                    int fromIndex = tour.Path[i].index;
                    int toIndex = tour.Path[i + 1].index;

                    if (fromIndex < 0 || toIndex < 0 || fromIndex >= NumberOfCities || toIndex >= NumberOfCities)
                        continue; // Consider a more efficient error handling strategy

                    if (fromIndex < DistanceMatrix.Count && toIndex < DistanceMatrix[fromIndex].Count)
                        totalDistance += DistanceMatrix[fromIndex][toIndex];

                    if (fromIndex < TimeMatrix.Count && toIndex < TimeMatrix[fromIndex].Count)
                        totalTime += TimeMatrix[fromIndex][toIndex];
                }

                // Handle the last city back to the start city
                int lastCityIndex = tour.Path.Last().index;
                int startCityIndex = tour.Path.First().index;

                if (lastCityIndex >= 0 && startCityIndex >= 0 && lastCityIndex < NumberOfCities && startCityIndex < NumberOfCities)
                {
                    if (lastCityIndex < DistanceMatrix.Count && startCityIndex < DistanceMatrix[lastCityIndex].Count)
                        totalDistance += DistanceMatrix[lastCityIndex][startCityIndex];

                    if (lastCityIndex < TimeMatrix.Count && startCityIndex < TimeMatrix[lastCityIndex].Count)
                        totalTime += TimeMatrix[lastCityIndex][startCityIndex];
                }

                // Weights for distance and time
                double weightDistance = 0.5; // Adjust these weights as needed
                double weightTime = 0.5;

                // Combined metric
                double combinedMetric = weightDistance * totalDistance + weightTime * totalTime;
                tour.Distance = combinedMetric; // You can use a different property if needed
                NumberOfEvaluations++;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error {0}", e.Message);
        }
    }

    private double CalculateDistance(City? from, City? to)
    {
        if (from == null || to == null)
            return double.MaxValue;

        if (DistanceType == DistanceType.Weighted && Weights != null)
        {
            return Weights[from.index - 1][to.index - 1]; // Adjust for zero-based index if needed
        }

        if (DistanceType == DistanceType.Euclidean)
        {
            return TspHelper.CalculateEuclideanDistance(from, to);
        }

        throw new InvalidOperationException("Unsupported distance type or missing weights data.");
    }

    public Tour GenerateTour()
    {
        if (!IsRealWorldData)
        {
            // Original implementation for generating tours using Cities list
            var shuffledCities = Cities.OrderBy(x => _random.Next()).ToList();
            var tour = new Tour(shuffledCities.Count);
            for (var i = 0; i < shuffledCities.Count; i++)
                tour.SetCity(i, shuffledCities[i]);
            return tour;
        }
        else
        {
            // Improved generation for real-world data
            var cityIndices = Enumerable.Range(0, DistanceMatrix.Count).OrderBy(x => _random.Next()).ToList();
            var tour = new Tour(cityIndices.Count);
            foreach (var index in cityIndices)
            {
                var city = Cities.FirstOrDefault(c => c.index == index) ??
                           new City { index = index, cordX = 0, cordY = 0 };
                tour.SetCity(index, city);
            }

            return tour;
        }
    }

    public int GetMaxEvaluations() => MaxEvaluations;

    public int GetNumberOfEvaluations() => NumberOfEvaluations;

    private class RealWorldData
    {
        public List<City> cities { get; set; }
        public List<List<int>> distanceMatrix { get; set; }
        public List<List<int>> timeMatrix { get; set; }
    }
}