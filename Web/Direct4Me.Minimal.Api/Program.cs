var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/postboxes/{postbox.Id}/unlock-nfc", () => "nfc");

app.MapGet("/postboxes/{postbox.Id}/unlock-qr", () => "qr");

app.Run();