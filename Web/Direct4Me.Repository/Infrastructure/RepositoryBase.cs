using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace Direct4Me.Repository.Infrastructure;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntity
{
    protected RepositoryBase(Direct4MeDbContext dbContext, IMongoDatabase mongoDatabase)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        MongoCollection = mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name.ToLower());
    }

    protected Direct4MeDbContext DbContext { get; }
    protected IMongoCollection<TEntity> MongoCollection { get; }

    public virtual async Task AddAsync(TEntity entity)
    {
        await DbContext.Set<TEntity>().AddAsync(entity);
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await MongoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await MongoCollection.Find(x => true).ToListAsync();
    }
}