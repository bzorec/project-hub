using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Direct4Me.Repository.Entities;

[Serializable]
public class PackageEntity : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public int PackageId { get; set; }

    public string PostBoxId { get; set; } = null!; // Associated postbox

    public string PackageType { get; set; } = null!; // e.g., perishable, electronics

    public double Weight { get; set; }

    public string DeliveryUrgency { get; set; } = null!; // e.g., standard, expedited

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}