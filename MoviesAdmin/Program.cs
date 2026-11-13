using Microsoft.EntityFrameworkCore;
using MoviesAdmin.Services;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Auth0 service
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"] ?? throw new InvalidOperationException("Configuration string 'Auth0:Domain' not found.");
    options.ClientId = builder.Configuration["Auth0:ClientId"] ?? throw new InvalidOperationException("Configuration string 'Auth0:ClientId' not found.");
    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Scope = "openid profile email";
    options.OpenIdConnectEvents = new OpenIdConnectEvents
    {
        OnTokenValidated = async context =>
        {
            var principal = context.Principal!;

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = principal.Identity?.Name;

            if (string.IsNullOrEmpty(userId)) return;

            var syncService = context.HttpContext.RequestServices
                .GetRequiredService<IUserSnapshotService>();

            await syncService.AddUserAsync(userId, email, name);
        }
    };
});

// Database service
var connectionString = builder.Configuration.GetConnectionString("MoviesAdminContext") ?? throw new InvalidOperationException("Connection string 'MoviesAdminContext' not found.");
builder.Services.AddDbContext<MoviesAdminContext>(options => options.UseSqlServer(connectionString));

// Asure Storage service
builder.Services.AddScoped<IAzureStorageService, AzureStorageService>();

// User Snapshot service
builder.Services.AddScoped<IUserSnapshotService,  UserSnapshotService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
