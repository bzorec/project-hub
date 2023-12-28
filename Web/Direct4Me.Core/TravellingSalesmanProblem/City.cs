namespace Direct4Me.Core.TravellingSalesmanProblem;

public class City
{
    public City()
    {
    }

    public City(City city)
    {
        if (city == null)
            throw new ArgumentNullException(nameof(city), "Cannot clone a null City.");

        Index = city.Index;
        CordX = city.CordX;
        CordY = city.CordY;
    }

    public int Index { get; set; }

    public double CordX { get; set; }

    public double CordY { get; set; }
}