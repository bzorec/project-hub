namespace Direct4Me.Core.TravellingSalesmanProblem;

public static class TspHelper
{
    public static double CalculateEuclideanDistance(City from, City to)
    {
        var deltaX = from.CordX - to.CordX;
        var deltaY = from.CordY - to.CordY;
        return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }

    public static double CalculateWeightedDistance(City from, City to, double weightFactor)
    {
        var euclideanDistance = CalculateEuclideanDistance(from, to);
        return euclideanDistance * weightFactor;
    }
}