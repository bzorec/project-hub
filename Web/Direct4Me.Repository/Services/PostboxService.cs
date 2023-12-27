using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Infrastructure;
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

    public async Task LogBoxUnlockAsync(string boxId, string type, bool isSuccess, CancellationToken token = default)
    {
        if (isSuccess)
        {
            var box = await GetPostboxByBoxIdAsync(boxId, token);

            box.UpdateUnlockCount(type);

            await UpdateAsync(box, token);
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

        postbox.StatisticsEntity.UpdateStatisticsDates();

        await _repository.UpdateAsync(postbox, token);

        return postbox;
    }

    public async Task<List<PostboxEntity>> GetAllAsync(string? userId, string? postboxId,
        CancellationToken token = default)
    {
        try
        {
            var filter = Builders<PostboxEntity>.Filter.Empty;

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

    public async Task<List<string>> GetOtherPostboxIdsForUser(string userId, CancellationToken token = default)
    {
        var postboxes = await _repository.GetAllAsync(token: token);
        var postboxIds = postboxes.Where(i => i.AccessList != null && i.AccessList.Contains(userId)).Select(p => p.Id).ToList();

        return postboxIds;
    }

    public async Task<PostboxEntity> GetPostboxByBoxIdAsync(string boxId, CancellationToken token = default)
    {
        var postboxes = await _repository.GetAllAsync(token: token);

        var postbox = postboxes.FirstOrDefault(i => i.PostBoxId.ToString() == boxId);

        postbox?.StatisticsEntity.UpdateStatisticsDates();

        await _repository.UpdateAsync(postbox ?? throw new InvalidOperationException(), token);

        return postbox;
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
}