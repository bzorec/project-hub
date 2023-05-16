using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Enums;

namespace Direct4Me.Repository.Services.Interfaces;

public interface IPostboxService
{
    Task UnlockPostboxAsync(string postboxId, UnlockType unlockType, CancellationToken token = default);
    Task<List<string>> GetPostboxIdsForUser(string userId, CancellationToken token = default);
    Task<PostboxEntity> GetPostboxByIdAsync(string postboxId, CancellationToken token = default);
    Task<List<PostboxEntity>> GetPostboxesByUserIdAsync(string userId, CancellationToken token = default);
    Task AddPostBoxAsync(string postBoxId, string userId, CancellationToken token = default);
    Task<List<PostboxEntity>> GetAllAsync(string? userId, string? postboxId);
    Task AddAsync(PostboxEntity postboxEntity);
    Task DeleteAsync(string guid);
    Task UpdateAsync(string guid);
}