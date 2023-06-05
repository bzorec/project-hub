using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Repositories.Interfaces;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Direct4Me.Repository.Services;

public class HistoryService : IHistoryService
{
    private readonly ILogger<HistoryService> _logger;
    private readonly IPostboxService _postboxService;
    private readonly IPostboxHistoryRepository _repository;
    private readonly IUserService _userService;

    public HistoryService(IPostboxHistoryRepository repository, ILogger<HistoryService> logger,
        IUserService userService, IPostboxService postboxService)
    {
        _repository = repository;
        _logger = logger;
        _userService = userService;
        _postboxService = postboxService;
    }

    public async Task<List<PostboxHistoryEntity>> GetFullHistorySingleAsync(string boxId,
        CancellationToken token = default)
    {
        try
        {
            var entity = await _postboxService.GetPostboxByBoxIdAsync(boxId, token);

            var postboxHistoryEntities = await _repository.GetAllAsync(token: token);

            return postboxHistoryEntities.Where(i => entity.PostBoxId.ToString().Contains(i.PostboxId)).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while getting history: {Message}", e.Message);
            return new List<PostboxHistoryEntity>();
        }
    }

    public async Task LogHistoryAsync(PostboxHistoryEntity entity, CancellationToken token = default)
    {
        try
        {
            await _repository.AddAsync(entity, token);
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while getting history: {Message}", e.Message);
        }
    }

    public async Task<List<PostboxHistoryEntity>> GetFullHistoryAsync(string userId, CancellationToken token = default)
    {
        try
        {
            var postboxId = await _postboxService.GetPostboxIdsForUser(userId, token);
            postboxId.AddRange(await _postboxService.GetOtherPostboxIdsForUser(userId, token));

            var result = await _repository.GetAllAsync(token: token);
            var postBoxIds = new List<string>();
            foreach (var id in postboxId)
            {
                var i = await _postboxService.GetPostboxByIdAsync(id, token);
                postBoxIds.Add(i.PostBoxId.ToString());
            }

            return result.Where(i => postBoxIds.Contains(i.PostboxId)).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while getting history: {Message}", e.Message);
            return new List<PostboxHistoryEntity>();
        }
    }
}