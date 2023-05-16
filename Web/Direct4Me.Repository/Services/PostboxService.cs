using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Repositories;
using MongoDB.Driver;

namespace Direct4Me.Repository.Services;

internal class PostboxService : IPostboxService
{
    private readonly IPostboxRepository _repository;

    public PostboxService(IPostboxRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<string>> GetPostboxIdsForUser(string userId, CancellationToken token)
    {
        var filter = Builders<PostboxEntity>.Filter.Eq("UserId", userId);

        var postboxes = await _repository.GetAllAsync(filter, token);
        var postboxIds = postboxes.Select(p => p.Id).ToList();

        return postboxIds;
    }

    public async Task<PostboxEntity> GetPostboxByIdAsync(string postboxId, CancellationToken token)
    {
        var postbox = await _repository.GetByIdAsync(postboxId, token);

        if (postbox == null)
            // Postbox not found, handle accordingly
            return null;

        // Update the statistics dates based on the current date
        postbox.StatisticsEntity.UpdateStatisticsDates();

        // Save the updated postbox back to the database
        await _repository.UpdateAsync(postbox, token);

        return postbox;
    }

    public async Task<List<PostboxEntity>> GetPostboxesByUserIdAsync(string userId, CancellationToken token = default)
    {
        var filter = Builders<PostboxEntity>.Filter.Eq(e => e.UserId, userId);

        var postboxes = await _repository.GetAllAsync(filter, token);

        if (postboxes == null)
            return null;

        var postboxEntities = postboxes.ToList();

        foreach (var box in postboxEntities)
        {
            box.StatisticsEntity.UpdateStatisticsDates();
            await _repository.UpdateAsync(box, token);
        }

        return postboxEntities;
    }

    public async Task AddPostBoxAsync(string postBoxId, string userId, CancellationToken token = default)
    {
        var entity = new PostboxEntity
        {
            PostBoxId = Convert.ToInt32(postBoxId),
            UserId = userId
        };
        await _repository.AddAsync(entity, token);
    }

    public async Task UnlockPostboxAsync(string postboxId, UnlockType unlockType, CancellationToken token = default)
    {
        // Retrieve the existing postbox from the repository
        var postbox = await _repository.GetByIdAsync(postboxId, token);

        if (postbox == null)
            // Postbox not found, handle accordingly
            return;

        // Update the unlock count based on the unlock type
        switch (unlockType)
        {
            case UnlockType.Nfc:
                postbox.StatisticsEntity.IncrementUnlockCount(true);
                break;
            case UnlockType.QrCode:
                postbox.StatisticsEntity.IncrementUnlockCount(false);
                break;
            default:
                return;
        }

        await _repository.UpdateAsync(postbox, token);
    }
}

public enum UnlockType
{
    Nfc,
    QrCode
}

public interface IPostboxService
{
    Task UnlockPostboxAsync(string postboxId, UnlockType unlockType, CancellationToken token = default);
    Task<List<string>> GetPostboxIdsForUser(string userId, CancellationToken token = default);
    Task<PostboxEntity> GetPostboxByIdAsync(string postboxId, CancellationToken token = default);
    Task<List<PostboxEntity>> GetPostboxesByUserIdAsync(string userId, CancellationToken token = default);
    Task AddPostBoxAsync(string postBoxId, string userId, CancellationToken token = default);
}