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

    public TspAlgorithm(string path, int maxEvaluations)
    {
        _random = new Random();
        MaxEvaluations = maxEvaluations;
        Cities = new List<City>();
        LoadData(path);
        NumberOfEvaluations = 0;
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
                        Index = int.Parse(parts[0]),
                        CordX = double.Parse(parts[1]),
                        CordY = double.Parse(parts[2])
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

                    var matrixLine = weight.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
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
                        Index = int.Parse(parts[0]),
                        CordX = double.Parse(parts[1]),
                        CordY = double.Parse(parts[2])
                    };
                    Cities.Add(city);
                }
            }
        }
    }

    public void Evaluate(Tour tour)
    {
        if (tour.Path == null || tour.Path.Count != NumberOfCities)
        {
            throw new InvalidOperationException("Tour path is not properly initialized or does not cover all cities.");
        }

        double distance = 0;

        // Calculate distance for each leg of the journey
        for (var index = 0; index < tour.Path.Count - 1; index++)
        {
            distance += CalculateDistance(tour.Path[index], tour.Path[index + 1]);
        }

        // Add distance from the last city back to the start city
        distance += CalculateDistance(tour.Path.Last(), Start);

        tour.Distance = distance;
        NumberOfEvaluations++;
    }

    private double CalculateDistance(City? from, City? to)
    {
        if (from == null || to == null)
            return double.MaxValue;

        if (DistanceType == DistanceType.Weighted && Weights != null)
        {
            return Weights[from.Index - 1][to.Index - 1]; // Adjust for zero-based index if needed
        }

        if (DistanceType == DistanceType.Euclidean)
        {
            return TspHelper.CalculateEuclideanDistance(from, to);
        }

        throw new InvalidOperationException("Unsupported distance type or missing weights data.");
    }

    public Tour GenerateTour()
    {
        var shuffledCities = Cities.OrderBy(x => _random.Next()).ToList();
        var tour = new Tour(shuffledCities.Count);

        for (var i = 0; i < shuffledCities.Count; i++) tour.SetCity(i, shuffledCities[i]);

        return tour;
    }

    public int GetMaxEvaluations() => MaxEvaluations;

    public int GetNumberOfEvaluations() => NumberOfEvaluations;
}