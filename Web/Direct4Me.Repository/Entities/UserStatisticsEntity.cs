namespace Direct4Me.Repository.Entities;

[Serializable]
public class UserStatisticsEntity
{
    public int DefaultLoginCount { get; set; }
    public int FaceLoginCount { get; set; }

    public WeeklyStatistics WeeklyStatistics { get; set; } = new();
    public MonthlyStatistics MonthlyStatistics { get; set; } = new();
    public DailyStatistics DailyStatistics { get; set; } = new();
    public DateTime LastModified { get; set; } = DateTime.Now;

    public void UpdateLoginStats(bool isFaceLogin)
    {
        var currentDate = DateTime.Now;

        // Update daily stats
        if (currentDate.Date != DailyStatistics.Date)
        {
            DailyStatistics.Date = currentDate;
            DailyStatistics.DefaultLoginCount = 0;
            DailyStatistics.FaceLoginCount = 0;
        }

        if (isFaceLogin)
        {
            DailyStatistics.FaceLoginCount++;
            FaceLoginCount++;
        }
        else
        {
            DailyStatistics.DefaultLoginCount++;
            DefaultLoginCount++;
        }

        // Update weekly stats
        if (currentDate.Date >= WeeklyStatistics.StartDate.AddDays(7))
        {
            WeeklyStatistics.StartDate = currentDate;
            WeeklyStatistics.DefaultLoginCount = 0;
            WeeklyStatistics.FaceLoginCount = 0;
        }

        if (isFaceLogin)
            WeeklyStatistics.FaceLoginCount++;
        else
            WeeklyStatistics.DefaultLoginCount++;

        // Update monthly stats
        if (currentDate.Date >= MonthlyStatistics.StartDate.AddMonths(1))
        {
            MonthlyStatistics.StartDate = currentDate;
            MonthlyStatistics.DefaultLoginCount = 0;
            MonthlyStatistics.FaceLoginCount = 0;
        }

        if (isFaceLogin)
            MonthlyStatistics.FaceLoginCount++;
        else
            MonthlyStatistics.DefaultLoginCount++;

        LastModified = currentDate;
    }
}

public sealed class WeeklyStatistics
{
    public int DefaultLoginCount { get; set; }
    public int FaceLoginCount { get; set; }
    public int TotalUnlocks => DefaultLoginCount + FaceLoginCount;

    public DateTime StartDate { get; set; }
}

public sealed class MonthlyStatistics
{
    public int DefaultLoginCount { get; set; }
    public int FaceLoginCount { get; set; }
    public int TotalUnlocks => DefaultLoginCount + FaceLoginCount;

    public DateTime StartDate { get; set; }
}

public sealed class DailyStatistics
{
    public int DefaultLoginCount { get; set; }
    public int FaceLoginCount { get; set; }
    public int TotalUnlocks => DefaultLoginCount + FaceLoginCount;

    public DateTime Date { get; set; }
}