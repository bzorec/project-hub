namespace Direct4Me.Core.TravellingSalesmanProblem;

public class TspAlgorithm
{
    private readonly Random _random;
    private readonly List<City> Cities = new();
    private readonly DistanceType DistanceType = DistanceType.Euclidean;
    private readonly int MaxEvaluations;
    private string? Name;
    private int NumberOfCities;
    private int NumberOfEvaluations;
    private City? Start;
    private double[][]? Weights;

    public TspAlgorithm(string path, int maxEvaluations)
    {
        LoadData(path);
        NumberOfEvaluations = 0;
        MaxEvaluations = maxEvaluations;
        _random = new Random(1);
    }

    public void Evaluate(Tour tour)
    {
        double distance = 0;
        distance += CalculateDistance(Start, tour.Path[0]);
        for (var index = 0; index < NumberOfCities; index++)
            if (index + 1 < NumberOfCities)
                distance += CalculateDistance(tour.Path[index], tour.Path[index + 1]);
            else
                distance += CalculateDistance(tour.Path[index], Start);
        tour.Distance = distance;
        NumberOfEvaluations++;
    }

    private double CalculateDistance(City? from, City? to)
    {
        switch (DistanceType)
        {
            case DistanceType.Euclidean:
                // Implement Euclidean distance calculation
                return 0;
            case DistanceType.Weighted:
                // Implement weighted distance calculation
                return 0;
            default:
                return double.MaxValue;
        }
    }

    public Tour GenerateTour()
    {
        var shuffledCities = Cities.OrderBy(x => _random.Next()).ToList();
        var tour = new Tour(shuffledCities.Count);

        for (var i = 0; i < shuffledCities.Count; i++) tour.SetCity(i, shuffledCities[i]);

        return tour;
    }

    public static TspData LoadData(string path)
    {
        var tspData = new TspData();

        try
        {
            var lines = File.ReadAllLines(path);
            var nodeSectionStarted = false;

            foreach (var line in lines)
                if (line.StartsWith("NAME"))
                {
                    tspData.Name = line.Split(new[] { ':' })[1].Trim();
                }
                else if (line.StartsWith("DIMENSION"))
                {
                    tspData.Dimension = int.Parse(line.Split(new[] { ':' })[1].Trim());
                }
                else if (line.StartsWith("NODE_COORD_SECTION"))
                {
                    nodeSectionStarted = true;
                }
                else if (nodeSectionStarted)
                {
                    if (line.StartsWith("EOF")) break;

                    var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var index = int.Parse(parts[0]);
                    var x = double.Parse(parts[1]);
                    var y = double.Parse(parts[2]);

                    tspData.Cities.Add(new City { Index = index, CordX = x, CordY = y });
                }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Error loading file " + path + ": " + e.Message);
        }

        return tspData;
    }

    public int GetMaxEvaluations()
    {
        return MaxEvaluations;
    }

    public int GetNumberOfEvaluations()
    {
        return NumberOfEvaluations;
    }
}