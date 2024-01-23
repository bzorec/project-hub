using Direct4Me.Core.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Direct4Me.Core.Modules;

public static class JavaRunnerModule
{
    public static IServiceCollection RegisterJavaRunner(this IServiceCollection serviceCollection) => serviceCollection
        .AddTransient<IJavaRunner, JavaRunner>();
}