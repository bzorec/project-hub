using System.Diagnostics.CodeAnalysis;
using Direct4Me.Minimal.Api.Infrastructure.Interfaces;
using Microsoft.OpenApi.Models;

namespace Direct4Me.Minimal.Api.EndpointDefinitions;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SwaggerEnpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "MinimalApi"));
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(x => { x.SwaggerDoc("v1", new OpenApiInfo {Title = "MinimalApi", Version = "v1"}); });
    }
}