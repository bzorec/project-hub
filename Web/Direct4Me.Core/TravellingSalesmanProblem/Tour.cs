namespace Direct4Me.Core.TravellingSalesmanProblem;

public class Tour
{
    public Tour(Tour tour)
    {
        Distance = tour.Distance;
        Dimension = tour.Dimension;
        Path = (City?[])tour.Path.Clone();
    }

    public Tour(int dimension)
    {
        Dimension = dimension;
        Path = new City[dimension];
        Distance = double.MaxValue;
    }

    public double Distance { get; set; }
    public int Dimension { get; }
    public City?[] Path { get; private set; }

    public Tour Clone()
    {
        return new Tour(this);
    }

    public void SetPath(City[] path)
    {
        Path = (City?[])path.Clone();
        Distance = double.MaxValue;
    }

    public void SetCity(int index, City? city)
    {
        Path[index] = city;
        Distance = double.MaxValue;
    }
}