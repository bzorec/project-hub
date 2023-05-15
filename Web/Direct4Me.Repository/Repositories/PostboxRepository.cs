using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Infrastructure;
using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace Direct4Me.Repository.Repositories;

public interface IPostboxRepository : IRepositoryBase<PostboxEntity>
{
}

internal class PostboxRepository : RepositoryBase<PostboxEntity>, IPostboxRepository
{
    public PostboxRepository(IMongoDatabase mongoDatabase)
        : base(mongoDatabase)
    {
    }
}