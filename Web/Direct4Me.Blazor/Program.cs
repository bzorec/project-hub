using Direct4Me.Blazor.Modules;
using Direct4Me.Blazor.Utils;
using Direct4Me.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureAppsettings();
builder.Services.ConfigureRepositoryServices();
builder.Services.ConfigureAuthServices(builder.Configuration);
builder.Services.AddJsInteropServices();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddTransient<HttpClient>();
builder.Services.AddTransient<IFaceRecognitionService, FaceRecognitionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();