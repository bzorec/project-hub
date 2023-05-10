namespace Direct4Me.Repository.Infrastructure.Interfaces;

public interface IRepositoryBase<TEntity>
{
    Task AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    Task<TEntity> GetByIdAsync(Guid id);

    Task<IEnumerable<TEntity>> GetAllAsync();
}