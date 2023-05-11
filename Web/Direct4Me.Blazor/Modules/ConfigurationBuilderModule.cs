namespace Direct4Me.Blazor.Modules;

public static class ConfigurationBuilderModule
{
    public static void ConfigureAppsettings(this IServiceCollection services)
    {
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", false, true);

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            configurationBuilder.AddJsonFile("appsettings.Development.json", true, true);

        IConfiguration configuration = configurationBuilder.Build();

        services.AddSingleton(configuration);
    }
}