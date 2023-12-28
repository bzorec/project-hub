namespace Direct4Me.Core.TravellingSalesmanProblem;

public class TspData
{
    public string Name { get; set; } = null!;
    public int Dimension { get; set; }
    public List<City> Cities { get; set; } = new();
}