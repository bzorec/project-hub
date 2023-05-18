using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Enums;

namespace Direct4Me.Repository.Services.Interfaces;

public interface IPostboxService
{
    Task UnlockPostboxAsync(string postboxId, UnlockType unlockType, CancellationToken token = default);

    Task<List<string>> GetPostboxIdsForUser(string userId, CancellationToken token = default);

    Task<PostboxEntity> GetPostboxByIdAsync(string postboxId, CancellationToken token = default);

    Task<List<PostboxEntity>> GetAllAsync(string? userId, string? postboxId, CancellationToken token = default);

    Task<bool> AddAsync(PostboxEntity postboxEntity, CancellationToken token = default);

    Task<bool> DeleteAsync(string guid, CancellationToken token = default);

    Task<bool> UpdateAsync(PostboxEntity entity, CancellationToken token = default);
}