using HomeNest.Components;
using HomeNest.Data;
using HomeNest.Data.Models;
using HomeNest.Services;
using Microsoft.EntityFrameworkCore;

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
            app.UseAntiforgery();
            app.MapStaticAssets();
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
}
