using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Infrastructure.Interfaces;

namespace Direct4Me.Repository.Repositories.Interfaces;

public interface IUserRepository : IRepositoryBase<UserEntity>
{
    Task<bool> DoseUserExistAsync(string email, CancellationToken token);
    Task<UserEntity> GetUserByEmailAsync(string email, CancellationToken token = default);
    Task<UserEntity?> GetUserByFullnameAsync(string firstname, string lastname, CancellationToken token);
}