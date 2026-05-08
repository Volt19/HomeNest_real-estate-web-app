using System.Security.Claims;
using HomeNest.Components;
using HomeNest.Data;
using HomeNest.Data.Models;
using HomeNest.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace HomeNest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Authentication & Authorization
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "homenest_session";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.SlidingExpiration = true;
                    options.LoginPath = "/login";
                });
            builder.Services.AddAuthorization();
            builder.Services.AddHttpContextAccessor();

            // Database - use factory for Blazor Server thread safety
            builder.Services.AddDbContextFactory<HomeNestDbContext>(options =>
                options.UseSqlite("Data Source=homenest.db")
                       .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

            // Services
            builder.Services.AddScoped<PropertyService>();
            builder.Services.AddScoped<UserStateService>();
            builder.Services.AddScoped<ContactService>();
            builder.Services.AddScoped<NewsletterService>();

            var app = builder.Build();

            // Ensure database is created and seeded
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<HomeNestDbContext>();
                db.Database.Migrate();
                SeedAdminUser(db);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery();
            app.MapStaticAssets();

            // Auth API endpoints
            app.MapPost("/api/auth/login", async (LoginRequest request, HomeNestDbContext db, HttpContext ctx) =>
            {
                var user = await db.Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return Results.Unauthorized();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Phone", user.Phone ?? ""),
                    new Claim("IsAdmin", user.IsAdmin.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await ctx.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                    });

                return Results.Ok(new { success = true });
            });

            app.MapPost("/api/auth/logout", async (HttpContext ctx) =>
            {
                await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.Ok(new { success = true });
            });

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }

        private static void SeedAdminUser(HomeNestDbContext db)
        {
            if (!db.Users.Any(u => u.Email == "admin@homenest.bg"))
            {
                db.Users.Add(new User
                {
                    Name = "Администратор",
                    Email = "admin@homenest.bg",
                    Phone = "+359 88 888 8888",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    IsAdmin = true,
                    CreatedAt = DateTime.UtcNow
                });
                db.SaveChanges();
            }
        }
    }

    public record LoginRequest(string Email, string Password);
}
