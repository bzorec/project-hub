using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Direct4Me.Repository.Entities;

public class PostboxEntity : IEntity
{
    [BsonIgnore] public PostboxStatisticsEntity StatisticsEntity { get; set; }

    public int PostBoxId { get; set; }

    public string PostBoxStatsId { get; set; }

    public string UserId { get; set; }

    [BsonIgnore] public UserEntity UserEntity { get; set; }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}