var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

// ✅ Test endpoint
app.MapGet("/test", () => "✅ API is running on Azure!");

app.Run();
