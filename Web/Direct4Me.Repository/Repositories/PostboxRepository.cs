using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Infrastructure;
using Direct4Me.Repository.Repositories.Interfaces;
using MongoDB.Driver;

namespace Direct4Me.Repository.Repositories;

internal class PostboxRepository : RepositoryBase<PostboxEntity>, IPostboxRepository
{
    public PostboxRepository(IMongoDatabase mongoDatabase)
        : base(mongoDatabase)
    {
    }
}