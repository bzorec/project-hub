using Direct4Me.Core.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace Direct4Me.Core.Modules;

public static class CommonModule
{
        public static IServiceCollection RegisterCommonServices(this IServiceCollection serviceCollection) => serviceCollection
            .AddTransient<IRouteHandler, Handler.RouteHandler>();
}