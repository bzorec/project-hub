namespace Direct4Me.Core.TravellingSalesmanProblem;

public class Tour
{
    public Tour(Tour tour)
    {
        Distance = tour.Distance;
        Dimension = tour.Dimension;
        Path = tour.Path.Select(city => city != null ? new City(city) : null).ToList();
    }

    public Tour(int dimension)
    {
        Dimension = dimension;
        Path = new List<City>(new City[dimension]);
        Distance = double.MaxValue;
    }

    public double Distance { get; set; }
    public int Dimension { get; }
    public List<City> Path { get; set; }

    public Tour Clone()
    {
        return new Tour(this);
    }

    public void SetPath(IEnumerable<City> path)
    {
        Path = new List<City>(path.Select(city => new City(city)));
        Distance = double.MaxValue;
    }

    public void SetCity(int index, City city)
    {
        while (index >= Path.Count) Path.Add(new City());

        Path[index] = new City(city);
        Distance = double.MaxValue;
    }
}