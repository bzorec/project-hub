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
    public double[][]? Weights { get; private set; } // If needed


    public TspAlgorithm(string path, int maxEvaluations)
    {
        _random = new Random();
        MaxEvaluations = maxEvaluations;
        LoadData(path);
        NumberOfEvaluations = 0;
    }

    private void LoadData(string path)
    {
        Cities = new List<City>();

        var lines = File.ReadAllLines(path);
        foreach (var line in lines)
        {
            if (line.StartsWith("NAME:"))
                Name = line.Split(':')[1].Trim();
            else if (line.StartsWith("DIMENSION:"))
                // Handle dimension line if needed
                continue;
            else if (line.StartsWith("NODE_COORD_SECTION"))
                break; // Start processing city data
        }

        foreach (var line in lines.SkipWhile(l => !l.StartsWith("NODE_COORD_SECTION")).Skip(1))
        {
            if (line == "EOF") break;

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var city = new City
            {
                Index = int.Parse(parts[0]),
                CordX = double.Parse(parts[1]),
                CordY = double.Parse(parts[2])
            };
            Cities.Add(city);
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

        return DistanceType switch
        {
            DistanceType.Euclidean => TspHelper.CalculateEuclideanDistance(from, to),
            DistanceType.Weighted => TspHelper.CalculateWeightedDistance(from, to, 1D),
            _ => double.MaxValue
        };
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