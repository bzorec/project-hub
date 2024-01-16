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

        index = city.index;
        cordX = city.cordX;
        cordY = city.cordY;
    }

    public int index { get; set; }

    public double cordX { get; set; }

    public double cordY { get; set; }
}