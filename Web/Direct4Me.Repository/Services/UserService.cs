using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Direct4Me.Repository.Services;

public interface IUserService
{
    Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken token);

    Task<UserEntity?> GetUserByFullnameAsync(string firstname, string lastname, CancellationToken token);

    Task<bool> TrySignInAsync(string email, string password, CancellationToken token);

    Task<bool> TrySignUpAsync(UserEntity entity, CancellationToken token);
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

            return user.Password == password;
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
}