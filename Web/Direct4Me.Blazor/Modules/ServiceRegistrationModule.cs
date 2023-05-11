using Direct4Me.Blazor.Data;
using Direct4Me.Blazor.Providers;

namespace Direct4Me.Blazor.Modules;

public static class ServiceRegistrationModule
{
    public static IServiceCollection AddModuleServices(this IServiceCollection sevices)
    {
        return sevices
            .AddSingleton<IUserService, UserService>()
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
            .AddSingleton<IJwtTokenProvider, JwtTokenProvider>();
    }
}