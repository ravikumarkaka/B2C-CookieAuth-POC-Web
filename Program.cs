var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "API is running ✅");
app.MapGet("/ping", () => "pong");

// Bind to the port Azure provides
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");
