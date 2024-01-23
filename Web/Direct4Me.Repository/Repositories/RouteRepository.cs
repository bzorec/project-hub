using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Infrastructure;
using Direct4Me.Repository.Repositories.Interfaces;
using MongoDB.Driver;

namespace Direct4Me.Repository.Repositories;

internal class RouteRepository : RepositoryBase<RouteEntity>, IRouteRepository
{
    public RouteRepository(IMongoDatabase mongoDatabase)
        : base(mongoDatabase)
    {
    }
}