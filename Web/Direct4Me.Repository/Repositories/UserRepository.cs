using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Infrastructure;
using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace Direct4Me.Repository.Repositories;

public interface IUserRepository : IRepositoryBase<UserEntity>
{
}

public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository(Direct4MeDbContext dbContext, IMongoDatabase mongoDatabase) : base(dbContext, mongoDatabase)
    {
    }
}