namespace Direct4Me.Minimal.Api.Infrastructure.Interfaces;

public interface IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app);

    public void DefineServices(IServiceCollection services);
}