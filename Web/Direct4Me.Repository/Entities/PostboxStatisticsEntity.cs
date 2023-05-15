namespace Direct4Me.Repository.Entities;

[Serializable]
public class PostboxStatisticsEntity
{
    public int NfcUnlock { get; set; }

    public int QrCodeUnlock { get; set; }

    public int TotalUnlocks => NfcUnlock + QrCodeUnlock;

    public DateTime LastModified { get; set; }

    public WeeklyPostboxStatistics WeeklyStatistics { get; set; } = new();
    public MonthlyPostboxStatistics MonthlyStatistics { get; set; } = new();
    public DailyPostboxStatistics DailyStatistics { get; set; } = new();

    public void UpdateStatisticsDates()
    {
        var currentDate = DateTime.Now;

        // Daily stats
        if (DailyStatistics.Date.Date != currentDate.Date)
        {
            DailyStatistics.Date = currentDate.Date;
            DailyStatistics.NfcUnlock = 0;
            DailyStatistics.QrCodeUnlock = 0;
        }

        // Weekly stats
        if (currentDate.Date > WeeklyStatistics.StartDate.AddDays(7))
        {
            WeeklyStatistics.StartDate = currentDate.Date;
            WeeklyStatistics.NfcUnlock = 0;
            WeeklyStatistics.QrCodeUnlock = 0;
        }

        // Monthly stats
        if (currentDate.Date > MonthlyStatistics.StartDate.AddMonths(1))
        {
            MonthlyStatistics.StartDate = currentDate.Date;
            MonthlyStatistics.NfcUnlock = 0;
            MonthlyStatistics.QrCodeUnlock = 0;
        }

        LastModified = currentDate;
    }

    public void IncrementUnlockCount(bool isNfcUnlock)
    {
        if (isNfcUnlock)
        {
            DailyStatistics.NfcUnlock++;
            WeeklyStatistics.NfcUnlock++;
            MonthlyStatistics.NfcUnlock++;
            NfcUnlock++;
        }
        else
        {
            DailyStatistics.QrCodeUnlock++;
            WeeklyStatistics.QrCodeUnlock++;
            MonthlyStatistics.QrCodeUnlock++;
            QrCodeUnlock++;
        }

        LastModified = DateTime.Now;
    }
}

public sealed class WeeklyPostboxStatistics
{
    public int NfcUnlock { get; set; }
    public int QrCodeUnlock { get; set; }
    public int TotalUnlocks => NfcUnlock + QrCodeUnlock;
    public DateTime StartDate { get; set; }
}

public sealed class MonthlyPostboxStatistics
{
    public int NfcUnlock { get; set; }
    public int QrCodeUnlock { get; set; }
    public int TotalUnlocks => NfcUnlock + QrCodeUnlock;
    public DateTime StartDate { get; set; }
}

public sealed class DailyPostboxStatistics
{
    public int NfcUnlock { get; set; }
    public int QrCodeUnlock { get; set; }
    public int TotalUnlocks => NfcUnlock + QrCodeUnlock;
    public DateTime Date { get; set; }
}