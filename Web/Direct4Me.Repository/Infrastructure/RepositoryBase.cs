using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace Direct4Me.Repository.Infrastructure;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntity
{
    protected RepositoryBase(IMongoDatabase mongoDatabase)
    {
        MongoDatabase = mongoDatabase ?? throw new ArgumentNullException(nameof(mongoDatabase));
        MongoCollection = MongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name.ToLower());
    }

    protected IMongoDatabase MongoDatabase { get; }
    protected IMongoCollection<TEntity> MongoCollection { get; }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken token)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
        await MongoCollection.ReplaceOneAsync(filter, entity, cancellationToken: token);
    }

    public virtual async Task DeleteAsync(string id, CancellationToken token)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
        await MongoCollection.DeleteOneAsync(filter, token);
    }

    public virtual async Task<TEntity> GetByIdAsync(string id, CancellationToken token)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
        return await MongoCollection.Find(filter).FirstOrDefaultAsync(token);
    }


    public virtual async Task AddAsync(TEntity entity, CancellationToken token)
    {
        await MongoCollection.InsertOneAsync(entity, cancellationToken: token);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
        FilterDefinition<TEntity>? filter = null,
        CancellationToken token = default)
    {
        filter ??= FilterDefinition<TEntity>.Empty;
        return await MongoCollection.Find(filter).ToListAsync(token);
    }
}