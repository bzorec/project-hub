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

    Task UpdateLoginCount(string email, LoginType loginType, CancellationToken token = default);
}

public class UserService : IUserService
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
            return user.CheckPassword(password);
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

    public async Task UpdateLoginCount(string email, LoginType loginType, CancellationToken token = default)
    {
        // Retrieve the existing user from the repository
        var user = await _repository.GetUserByEmailAsync(email, token);

        if (user == null)
            // User not found, handle accordingly
            return;

        // Update the login count based on the login type
        switch (loginType)
        {
            case LoginType.Default:
                user.StatisticsEntity.DefaultLoginCount++;
                break;
            case LoginType.Face:
                user.StatisticsEntity.FaceLoginCount++;
                break;
            default:
                // Invalid login type, handle accordingly
                return;
        }

        // Update the last modified date
        user.StatisticsEntity.LastModified = DateTime.Now;

        // Save the modified user back to the repository
        await _repository.UpdateAsync(user, token);
    }
}

public enum LoginType
{
    Default,
    Face
}