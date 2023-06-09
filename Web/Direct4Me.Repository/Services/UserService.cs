using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Enums;
using Direct4Me.Repository.Repositories.Interfaces;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace Direct4Me.Repository.Services;

internal class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository, ILogger<UserService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> AddAsync(UserEntity userEntity, CancellationToken token = default)
    {
        try
        {
            await _repository.AddAsync(userEntity, token);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while adding user: {Message}", e.Message);
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
            _logger.LogError("Error occured while updating user: {Message}", e.Message);
            return false;
        }
    }

    public async Task<bool> UpdateAsync(UserEntity userEntity, CancellationToken token = default)
    {
        try
        {
            await _repository.UpdateAsync(userEntity, token);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while updating user: {Message}", e.Message);
            return false;
        }
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken token)
    {
        try
        {
            return await _repository.GetUserByEmailAsync(email, token);
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while getting user by email: {Excepiton}", e);

            return null;
        }
    }

    public async Task<UserEntity?> GetUserByIdAsync(string id, CancellationToken token = default)
    {
        try
        {
            return await _repository.GetByIdAsync(id, token);
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while getting user by email: {Excepiton}", e);

            return null;
        }
    }

    public async Task<bool> TrySignInAsync(string email, string password, CancellationToken token)
    {
        if (!await _repository.DoseUserExistAsync(email, token)) return false;

        try
        {
            var user = await _repository.GetUserByEmailAsync(email, token);


            var result = user.CheckPassword(password);

            if (result)
                await UpdateLoginCount(user, LoginType.Default, token);

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while adding user: {Excepiton}", e);

            return false;
        }
    }

    public async Task<bool> TrySignUpAsync(UserEntity entity, CancellationToken token)
    {
        if (await _repository.DoseUserExistAsync(entity.Email, token)) return false;

        try
        {
            await _repository.AddAsync(entity, token);

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while adding user: {Excepiton}", e);

            return false;
        }
    }

    public async Task<List<UserEntity>> GetAllAsync(string? firstname, string? lastname, DateTime? lastAccessed,
        CancellationToken token = default)
    {
        try
        {
            FilterDefinition<UserEntity>? filter = FilterDefinition<UserEntity>.Empty;

            if (!firstname.IsNullOrEmpty())
                filter &= Builders<UserEntity>.Filter.Eq(e => e.FirstName, firstname);

            if (!lastname.IsNullOrEmpty())
                filter &= Builders<UserEntity>.Filter.Eq(e => e.LastName, lastname);

            if (lastAccessed != null)
                filter &= Builders<UserEntity>.Filter.Eq(e => e.StatisticsEntity.LastModified, lastAccessed);

            var list = await _repository.GetAllAsync(filter, token);

            return list.ToList();
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while retriving data: {Message}", e.Message);
            return new List<UserEntity>();
        }
    }

    private async Task UpdateLoginCount(UserEntity user, LoginType loginType, CancellationToken token = default)
    {
        if (user == null)
            return;

        switch (loginType)
        {
            case LoginType.Default:
                user.StatisticsEntity.UpdateLoginStats(false);
                break;
            case LoginType.Face:
                user.StatisticsEntity.UpdateLoginStats(true);
                break;
            default:
                return;
        }

        await _repository.UpdateAsync(user, token);
    }
}