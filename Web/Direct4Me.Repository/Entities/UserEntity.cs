using System.Security.Cryptography;
using System.Text;
using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Direct4Me.Repository.Entities;

[Serializable]
public class UserEntity : IEntity
{
    [BsonIgnore] private string? _password;

    public UserStatisticsEntity StatisticsEntity { get; set; } = new();

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Fullname => $"{FirstName} {LastName}";

    public string Password
    {
        get => _password;
        set
        {
            // Check if the value matches the expected hashed password format
            bool isAlreadyHashed = IsAlreadyHashed(value);

            // If the value is already hashed, assign it directly to _password without rehashing
            if (isAlreadyHashed)
            {
                _password = value;
            }
            else
            {
                // Hash the password using SHA-256 and then convert it to Base64 string for storage
                var passwordHash = HashPassword(value);
                _password = Convert.ToBase64String(passwordHash);
            }
        }
    }

    public bool IsFaceUnlock { get; set; }

    [BsonElement]
    [BsonRepresentation(BsonType.Binary)]
    public byte[]? Image { get; set; }


    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public static bool IsAlreadyHashed(string value)
    {
        byte[] hashedBytes;
        try
        {
            // Convert the provided value from Base64 string to bytes
            hashedBytes = Convert.FromBase64String(value);
        }
        catch (FormatException)
        {
            // If the provided value is not a valid Base64 string, it is not a hashed password
            return false;
        }

        // Compare the length of the hashed bytes with the expected hash length (SHA-256 produces a 32-byte hash)
        return hashedBytes.Length == 32;
        // You can perform additional checks if needed, such as specific byte values or patterns
        // If all checks pass, consider it as already hashed
    }

    public bool CheckPassword(string passwordToCheck)
    {
        // Hash the incoming password using SHA-256 and then convert it to Base64 string
        var passwordHashToCheck = HashPassword(passwordToCheck.Trim());
        var passwordToCheckBase64 = Convert.ToBase64String(passwordHashToCheck);

        // Compare the stored password hash with the incoming password hash
        return _password == passwordToCheckBase64;
    }

    public byte[] HashPassword(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        return SHA256.HashData(passwordBytes);
    }
}