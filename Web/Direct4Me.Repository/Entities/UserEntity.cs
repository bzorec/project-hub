using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Direct4Me.Repository.Entities;

public class UserEntity : IEntity
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}