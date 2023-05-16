using Direct4Me.Minimal.Api.Infrastructure.Interfaces;

namespace Direct4Me.Minimal.Api.Infrastructure;

public static class EndpointDefinitionExtensions
{
    public static void AddEndpointDefinitions(this IServiceCollection services, params Type[] scanMarkers)
    {
        var endpointDefinitions = new List<IEndpointDefinition>();

        foreach (var marker in scanMarkers)
            endpointDefinitions.AddRange(marker.Assembly.ExportedTypes
                .Where(type =>
                    typeof(IEndpointDefinition).IsAssignableFrom(type) && type is {IsClass: true, IsAbstract: false})
                .Select(Activator.CreateInstance)
                .Cast<IEndpointDefinition>());

        foreach (var definition in endpointDefinitions) definition.DefineServices(services);

        services.AddSingleton<IReadOnlyCollection<IEndpointDefinition>>(endpointDefinitions);
    }


    public static void UseEndpointDefinitions(this WebApplication app)
    {
        var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>();

        foreach (var definition in definitions) definition.DefineEndpoints(app);
    }
}