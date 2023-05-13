using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Repositories;
using MongoDB.Driver;

namespace Direct4Me.Repository.Services;

public class PostboxService : IPostboxService
{
    private readonly IPostboxRepository _postboxRepository;
    private readonly IPostboxStatisticsRepository _postboxStatisticsRepository;

    public PostboxService(IPostboxRepository postboxRepository,
        IPostboxStatisticsRepository postboxStatisticsRepository)
    {
        _postboxRepository = postboxRepository;
        _postboxStatisticsRepository = postboxStatisticsRepository;
    }

    public async Task<PostboxEntity> GetPostboxAsync(string postboxId, CancellationToken token)
    {
        var postbox = await _postboxRepository.GetByIdAsync(postboxId, token);

        if (postbox == null) return postbox;

        // Update the statistics of the PostboxEntity
        var currentDate = DateTime.UtcNow;

        // Retrieve the corresponding PostboxStatisticsEntity
        var postboxStatistics =
            await _postboxStatisticsRepository.GetByIdAsync(postbox.StatisticsEntity.Id, token);

        // Update the unlock statistics based on the logic
        var weeklyStatistics = postboxStatistics.WeeklyStatistics;
        if (currentDate.Date >= weeklyStatistics.StartDate.AddDays(7).Date)
        {
            // Reset the weekly unlock statistics for the new week
            weeklyStatistics.NfcUnlock = 0;
            weeklyStatistics.QrCodeUnlock = 0;
            weeklyStatistics.TotalUnlocks = 0;
            weeklyStatistics.StartDate = currentDate.Date;
        }

        var monthlyStatistics = postboxStatistics.MonthlyStatistics;
        if (currentDate.Date >= monthlyStatistics.StartDate.AddMonths(1).Date)
        {
            // Reset the monthly unlock statistics for the new month
            monthlyStatistics.NfcUnlock = 0;
            monthlyStatistics.QrCodeUnlock = 0;
            monthlyStatistics.TotalUnlocks = 0;
            monthlyStatistics.StartDate = currentDate.Date;
        }

        var dailyStatistics = postboxStatistics.DailyStatistics;
        if (currentDate.Date > dailyStatistics.Date.Date)
        {
            // Reset the daily unlock statistics for the new day
            dailyStatistics.NfcUnlock = 0;
            dailyStatistics.QrCodeUnlock = 0;
            dailyStatistics.TotalUnlocks = 0;
            dailyStatistics.Date = currentDate.Date;
        }

        // Save the updated PostboxStatisticsEntity back to the database
        await _postboxStatisticsRepository.UpdateAsync(postboxStatistics, token);

        postbox.StatisticsEntity = postboxStatistics;

        return postbox;
    }

    public async Task<List<string>> GetPostboxIdsForUser(string userId, CancellationToken token)
    {
        var filter = Builders<PostboxEntity>.Filter.Eq("UserId", userId);

        var postboxes = await _postboxRepository.GetAllAsync(filter, token);
        var postboxIds = postboxes.Select(p => p.Id).ToList();

        return postboxIds;
    }
}

public interface IPostboxService
{
    Task<List<string>> GetPostboxIdsForUser(string userId, CancellationToken token = default);
    Task<PostboxEntity> GetPostboxAsync(string postboxId, CancellationToken token = default);
}