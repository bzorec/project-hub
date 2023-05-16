using Direct4Me.Repository.Entities;

namespace Direct4Me.Repository.Services.Interfaces;

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