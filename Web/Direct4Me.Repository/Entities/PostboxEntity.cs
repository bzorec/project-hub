using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Direct4Me.Repository.Entities;

[Serializable]
public class PostboxEntity : IEntity
{
    public PostboxStatisticsEntity StatisticsEntity { get; set; } = new();

    public int PostBoxId { get; set; }

    public string UserId { get; set; } = null!;

    public List<string>? AccessList { get; set; } = new();

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}