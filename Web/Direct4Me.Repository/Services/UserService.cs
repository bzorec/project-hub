using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Direct4Me.Repository.Services;

public interface IUserService
{
    Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken token = default);

    Task<UserEntity?> GetUserByFullnameAsync(string firstname, string lastname, CancellationToken token = default);

    Task<bool> TrySignInAsync(string email, string password, CancellationToken token = default);

    Task<bool> TrySignUpAsync(UserEntity entity, CancellationToken token = default);
    Task<List<UserEntity>> GetAllAsync();

    Task<bool> AddAsync(
        UserEntity userEntity);

    Task DeleteAsync(string guid);
    Task UpdateAsync(UserEntity userEntity);
}

internal class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository, ILogger<UserService> logger)
    {
        _repository = repository;
        _logger = logger;
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

    public async Task<UserEntity?> GetUserByFullnameAsync(string firstname, string lastname,
        CancellationToken token)
    {
        if (firstname.IsNullOrEmpty() && lastname.IsNullOrEmpty())
            return null;

        try
        {
            return await _repository.GetUserByFullnameAsync(firstname, lastname, token);
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while adding user: {Excepiton}", e);

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

    public async Task<List<UserEntity>> GetAllAsync()
    {
        try
        {
            var list = await _repository.GetAllAsync();

            return list.ToList();
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while retriving data: {Message}", e.Message);
            return new List<UserEntity>();
        }
    }

    public Task<bool> AddAsync(UserEntity userEntity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string guid)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(UserEntity userEntity)
    {
        throw new NotImplementedException();
    }

    private async Task UpdateLoginCount(UserEntity user, LoginType loginType, CancellationToken token = default)
    {
        if (user == null)
            // User not found, handle accordingly
            return;

        // Update the login count based on the login type
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

public enum LoginType
{
    Default,
    Face
}