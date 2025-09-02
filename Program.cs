var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Enable serving static files (wwwroot folder)
app.UseStaticFiles();

// Enable routing + endpoints
app.UseRouting();

app.MapGet("/", () => "Hello, ASP.NET Core API is running 🚀");

// Your other endpoints
// app.MapControllers();  // if using controllers
// app.MapRazorPages();   // if using Razor pages

app.Run();
