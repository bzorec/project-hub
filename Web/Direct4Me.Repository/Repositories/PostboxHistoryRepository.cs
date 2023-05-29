using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Infrastructure;
using Direct4Me.Repository.Repositories.Interfaces;
using MongoDB.Driver;

namespace Direct4Me.Repository.Repositories;

internal class PostboxHistoryRepository : RepositoryBase<PostboxHistoryEntity>, IPostboxHistoryRepository
{
    public PostboxHistoryRepository(IMongoDatabase mongoDatabase)
        : base(mongoDatabase)
    {
    }
}