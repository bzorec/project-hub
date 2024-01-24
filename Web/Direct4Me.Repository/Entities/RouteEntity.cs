using Direct4Me.Repository.Infrastructure.Interfaces;

namespace Direct4Me.Repository.Entities;

public class RouteEntity
{
    public string Id { get; set; } = null!;

    public List<PostboxEntity> Postboxes { get; set; } = new(); // List of postboxes in the route

    public double EstimatedTravelTime { get; set; } // In hours or minutes
    public double TotalDistance { get; set; } // In kilometers or miles

    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public List<List<int>> DistanceMatrix { get; set; }
}