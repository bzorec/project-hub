using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Infrastructure;
using Direct4Me.Repository.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace Direct4Me.Repository.Repositories;

public class PostboxStatisticsRepository : RepositoryBase<PostboxStatisticsEntity>, IPostboxStatisticsRepository
{
    public PostboxStatisticsRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }
}

public interface IPostboxStatisticsRepository : IRepositoryBase<PostboxStatisticsEntity>
{
}