using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Direct4Me.Repository;

public class Direct4MeDbContextSettings
{
    public Direct4MeDbContextSettings(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("MongoDb");

        MongoClientSettings = MongoClientSettings.FromConnectionString(ConnectionString);
    }

    public string? ConnectionString { get; }
    public MongoClientSettings MongoClientSettings { get; }
}