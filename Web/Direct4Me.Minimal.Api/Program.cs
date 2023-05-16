using Direct4Me.Minimal.Api.Infrastructure;
using Direct4Me.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureRepositoryServices();

//TODO:bzorec make this so it's doesn't uses magic to add services to collection
builder.Services.AddEndpointDefinitions(typeof(EndpointDefinitionExtensions));

var app = builder.Build();
app.UseEndpointDefinitions();
app.Run();