using MongoDB.Driver;

namespace Direct4Me.Repository.Infrastructure.Interfaces;

public interface IRepositoryBase<TEntity>
{
    Task AddAsync(TEntity entity, CancellationToken token = default);

    Task UpdateAsync(TEntity entity, CancellationToken token = default);

    Task DeleteAsync(string id, CancellationToken token = default);

    Task<TEntity> GetByIdAsync(string id, CancellationToken token = default);

    Task<IEnumerable<TEntity>> GetAllAsync(
        FilterDefinition<TEntity>? filter = default,
        CancellationToken token = default);
}