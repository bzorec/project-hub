using System.Security.Cryptography;
using System.Text;
using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Direct4Me.Repository.Entities;

public class UserEntity : IEntity
{
    private string _password;
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Fullname => $"{FirstName} {LastName}";

    public string Password
    {
        get
        {
            // Decrypt the password using Base64 decoding and UTF-8 encoding
            var passwordBytes = Convert.FromBase64String(_password);
            return Encoding.UTF8.GetString(passwordBytes);
        }
        set
        {
            // Hash the password using SHA-256
            var passwordHash = SHA256.HashData(Encoding.UTF8.GetBytes(value));
            _password = Convert.ToBase64String(passwordHash);
        }
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}