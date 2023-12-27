using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Direct4Me.Repository.Entities;

public class PostboxHistoryEntity : IEntity
{
    public DateTime Date { get; set; }

    public string UserName { get; set; } = null!;

    public string PostboxId { get; set; } = null!;
    public string? Type { get; set; }

    public bool Success { get; set; }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}