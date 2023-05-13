using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Direct4Me.Repository.Entities;

public class PostboxStatisticsEntity : IEntity
{
    [BsonIgnore] public int NfcUnlock { get; set; }

    [BsonIgnore] public int QrCodeUnlock { get; set; }

    [BsonIgnore] public int TotalUnlocks { get; set; }

    public WeeklyStatistics WeeklyStatistics { get; set; } = new();
    public MonthlyStatistics MonthlyStatistics { get; set; } = new();
    public DailyStatistics DailyStatistics { get; set; } = new();

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}

public class WeeklyStatistics
{
    public int NfcUnlock { get; set; }
    public int QrCodeUnlock { get; set; }
    public int TotalUnlocks { get; set; }
    public DateTime StartDate { get; set; }
}

public class MonthlyStatistics
{
    public int NfcUnlock { get; set; }
    public int QrCodeUnlock { get; set; }
    public int TotalUnlocks { get; set; }
    public DateTime StartDate { get; set; }
}

public class DailyStatistics
{
    public int NfcUnlock { get; set; }
    public int QrCodeUnlock { get; set; }
    public int TotalUnlocks { get; set; }
    public DateTime Date { get; set; }
}