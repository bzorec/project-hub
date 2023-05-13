var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/postboxes/{postbox.Id}/unlock-nfc", (string postbox) => $"nfc {postbox}");

app.MapGet("/postboxes/{postbox.Id}/unlock-qr", (string postbox) => $"qr {postbox}");

app.Run();