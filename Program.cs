var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "API is running ✅");
app.MapGet("/ping", () => "pong");

app.Run();
