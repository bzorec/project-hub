using Serilog;
using Serilog.Formatting.Compact;
using Direct4Me.Minimal.Api.Infrastructure;
using Direct4Me.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureRepositoryServices();
// builder.Services
//     .AddTransient<IImageProcessingService>()
//     .AddTransient<IPythonAiService>();

builder.Services.AddEndpointDefinitions(typeof(EndpointDefinitionExtensions));
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.File(new CompactJsonFormatter(), "logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseEndpointDefinitions();
app.Run();