using MongoDB.Driver;

namespace Direct4Me.Repository.Infrastructure.Interfaces;

public interface IRepositoryBase<TEntity>
{
    Task AddAsync(TEntity entity, CancellationToken token);

    Task UpdateAsync(TEntity entity, CancellationToken token);

    Task DeleteAsync(TEntity entity, CancellationToken token);

    Task<TEntity> GetByIdAsync(string id, CancellationToken token);

    Task<IEnumerable<TEntity>> GetAllAsync(
        FilterDefinition<TEntity> filter = default,
        CancellationToken token = default);
}