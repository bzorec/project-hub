using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Direct4Me.Repository;

public static class Direct4MeDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder<Direct4MeDbContext> UseMongoDb(
        this DbContextOptionsBuilder<Direct4MeDbContext> optionsBuilder,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Direct4MeMongoDb");

        var mongoClientSettings = MongoClientSettings.FromConnectionString(connectionString);
        mongoClientSettings.ClusterConfigurator = cb =>
        {
            // configure your MongoDB cluster here if needed
        };

        return optionsBuilder.UseMongoDb(connectionString, mongoClientSettings);
    }

    public static DbContextOptionsBuilder<Direct4MeDbContext> UseMongoDb(
        this DbContextOptionsBuilder<Direct4MeDbContext> optionsBuilder,
        string? connectionString,
        MongoClientSettings mongoClientSettings)
    {
        return optionsBuilder;
    }
}