using Microsoft.AspNetCore.Authentication.Cookies;


// Authentication: Cookie + OIDC (Azure AD B2C)
builder.Services.AddAuthentication(options =>
{
options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
// Adjust cookie settings for POC / App Service (secure by default)
options.Cookie.HttpOnly = true;
options.Cookie.SameSite = SameSiteMode.None; // if cross-site scenarios (adjust for your scenario)
options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
})
.AddMicrosoftIdentityWebApp(azureAdB2CSection);


builder.Services.AddAuthorization();


// Minimal API endpoints
var app = builder.Build();


app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();


// Health / public
app.MapGet("/", () => Results.Content("<h2>Global Nav POC (Cookie Auth) — Public</h2>", "text/html"));


// Login end-point: triggers B2C OIDC flow
app.MapGet("/login", async (HttpContext http) =>
{
await http.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new Microsoft.AspNetCore.Authentication.AuthenticationProperties
{
RedirectUri = "/"
});
});


// Logout: sign out from cookie and B2C
app.MapGet("/logout", async (HttpContext http) =>
{
// Sign out from local cookie
await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


// Then sign out from the OpenIdConnect (B2C) to clear the session at provider (redirect back to home)
await http.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new Microsoft.AspNetCore.Authentication.AuthenticationProperties
{
RedirectUri = "/"
});
});


// Protected endpoint
app.MapGet("/secure", (ClaimsPrincipal user) =>
{
var name = user.Identity?.Name ?? user.FindFirst("name")?.Value ?? "(unknown)";
return Results.Json(new { message = $"Welcome {name} — authenticated via Azure AD B2C cookie." });
}).RequireAuthorization();


// Return user claims for debugging (only for POC)
app.MapGet("/claims", (ClaimsPrincipal user) =>
{
var claims = user.Claims.Select(c => new { c.Type, c.Value });
return Results.Json(claims);
}).RequireAuthorization();


app.Run();
