using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Infrastructure;
using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace Direct4Me.Repository.Repositories;

public interface IUserRepository : IRepositoryBase<UserEntity>
{
    Task<bool> DoseUserExistAsync(string email, CancellationToken token);
    Task<UserEntity> GetUserByEmailAsync(string email, CancellationToken token);
    Task<UserEntity?> GetUserByFullnameAsync(string firstname, string lastname, CancellationToken token);
}

internal class UserRepository : RepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    public async Task<bool> DoseUserExistAsync(string email, CancellationToken token)
    {
        var filter = Builders<UserEntity>.Filter.Eq(e => e.Email, email);

        return await MongoCollection.Find(filter).AnyAsync(token);
    }

    public async Task<UserEntity> GetUserByEmailAsync(string email, CancellationToken token)
    {
        var filter = Builders<UserEntity>.Filter.Eq(e => e.Email, email);

        return await MongoCollection.Find(filter).FirstOrDefaultAsync(token);
    }

    public async Task<UserEntity?> GetUserByFullnameAsync(string firstname, string lastname, CancellationToken token)
    {
        var filter = Builders<UserEntity>.Filter.And(
            Builders<UserEntity>.Filter.Eq(e => e.FirstName, firstname),
            Builders<UserEntity>.Filter.Eq(e => e.LastName, lastname));

        return await MongoCollection.Find(filter).FirstOrDefaultAsync(token);
    }
}