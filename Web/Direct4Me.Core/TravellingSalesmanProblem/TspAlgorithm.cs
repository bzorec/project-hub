namespace Direct4Me.Core.TravellingSalesmanProblem;

public class TspAlgorithm
{
    private readonly DistanceType DistanceType = DistanceType.Euclidean;
    private readonly int MaxEvaluations;
    private List<City> Cities = new();
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
        //TODO: Implement random tour generation logic
        return null;
    }

    private void LoadData(string path)
    {
        //TODO: Set starting city, which is always at index 0

        string[] lines;
        try
        {
            lines = File.ReadAllLines(path);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("File " + path + " not found!");
        }

        //TODO: Parse data from lines
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