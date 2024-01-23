using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Direct4Me.Repository.Entities;

[Serializable]
public class RouteEntity : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public List<PostboxEntity> Postboxes { get; set; } = new(); // List of postboxes in the route

    public double EstimatedTravelTime { get; set; } // In hours or minutes
    public double TotalDistance { get; set; } // In kilometers or miles

    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}