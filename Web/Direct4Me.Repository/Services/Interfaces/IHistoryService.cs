using Direct4Me.Repository.Entities;

namespace Direct4Me.Repository.Services.Interfaces;

public interface IHistoryService
{
    Task<List<PostboxHistoryEntity>> GetFullHistoryAsync(string userId, CancellationToken token = default);
    Task<List<PostboxHistoryEntity>> GetFullHistorySingleAsync(string boxId, CancellationToken token = default);

    Task LogHistoryAsync(PostboxHistoryEntity entity, CancellationToken token = default);
}