using Direct4Me.Repository.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Direct4Me.Repository;

public static class StartupExstention
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("MongoDB");

            var mongoUrl = MongoUrl.Create(connectionString);
            var client = new MongoClient(mongoUrl);

            return client.GetDatabase(configuration["MongoDbSettings:DbName"]);
        });

        services.AddSingleton<IUserRepository, UserRepository>();
    }
}