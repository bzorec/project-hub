using Direct4Me.Repository.Entities;

namespace Direct4Me.Repository.Services.Interfaces;

public interface IUserService
{
    Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken token = default);
    Task<UserEntity?> GetUserByIdAsync(string id, CancellationToken token = default);

    Task<bool> TrySignInAsync(string email, string password, CancellationToken token = default);

    Task<bool> TrySignUpAsync(UserEntity entity, CancellationToken token = default);

    Task<List<UserEntity>> GetAllAsync(string? firstname, string? lastname, DateTime? lastAccessed,
        CancellationToken token = default);

    Task<bool> AddAsync(UserEntity userEntity, CancellationToken token = default);

    Task<bool> DeleteAsync(string guid, CancellationToken token = default);
    Task<bool> UpdateAsync(UserEntity userEntity, CancellationToken token = default);

    Task<UserEntity> GetAsync(string? id, string? email, string? firstname, string? lastname,
        DateTime? lastAccessed,
        CancellationToken token = default);
}