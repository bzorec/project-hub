using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Enums;
using Direct4Me.Repository.Repositories.Interfaces;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace Direct4Me.Repository.Services;

internal class PostboxService : IPostboxService
{
    private readonly ILogger<PostboxService> _logger;
    private readonly IPostboxRepository _repository;

    public PostboxService(IPostboxRepository repository, ILogger<PostboxService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> AddAsync(PostboxEntity postboxEntity, CancellationToken token = default)
    {
        try
        {
            await _repository.AddAsync(postboxEntity, token);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while adding postbox: {Message}", e.Message);
            return false;
        }
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

    public async Task<List<PostboxEntity>> GetAllAsync(string? userId, string? postboxId,
        CancellationToken token = default)
    {
        try
        {
            FilterDefinition<PostboxEntity>? filter = Builders<PostboxEntity>.Filter.Empty;

            if (!userId.IsNullOrEmpty())
                filter &= Builders<PostboxEntity>.Filter.Eq(e => e.UserId, userId);

            if (!postboxId.IsNullOrEmpty())
                filter &= Builders<PostboxEntity>.Filter.Eq(e => e.PostBoxId, Convert.ToInt32(postboxId));

            var result = await _repository.GetAllAsync(filter, token);

            return result.ToList();
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while retriving postboxes: {Message}", e.Message);
            return new List<PostboxEntity>();
        }
    }

    public async Task<bool> UpdateAsync(PostboxEntity entity, CancellationToken token = default)
    {
        try
        {
            await _repository.UpdateAsync(entity, token);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while updating postbox: {Message}", e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(string guid, CancellationToken token = default)
    {
        try
        {
            await _repository.DeleteAsync(guid, token);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while deleting postbox: {Message}", e.Message);
            return false;
        }
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
}