using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Direct4Me.Repository.Entities;

public class PackageEntity
{
    public string Id { get; set; } = null!;

    public int PackageId { get; set; }

    public int PostBoxId { get; set; } // Now an integer

    public string PackageType { get; set; } = null!;
    public double Weight { get; set; }
    public string DeliveryUrgency { get; set; } = null!;
    public string DayOfWeek { get; set; } = null!; // Added for Java compatibility
    public string PaymentType { get; set; } = null!; // Added for Java compatibility
    public string NeedSignature { get; set; } = null!; // Added for Java compatibility
    public int NumPeopleHome { get; set; } // Added for Java compatibility
    public int PostboxCapacity { get; set; } // Added for Java compatibility
    public string RepeatDelivery { get; set; } = null!; // Added for Java compatibility

    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}