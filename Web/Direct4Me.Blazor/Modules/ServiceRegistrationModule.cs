using Direct4Me.Blazor.Providers;

namespace Direct4Me.Blazor.Modules;

public static class ServiceRegistrationModule
{
    public static IServiceCollection AddModuleServices(this IServiceCollection sevices)
    {
        return sevices
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
            .AddSingleton<IJwtTokenProvider, JwtTokenProvider>();
    }
}