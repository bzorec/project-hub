using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Direct4Me.Repository.Infrastructure.Interfaces;

public interface IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}