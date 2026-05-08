using HomeNest.Data;
using HomeNest.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeNest.Services;

public class UserStateService
{
    private readonly IDbContextFactory<HomeNestDbContext> _factory;

    public bool IsLoggedIn { get; private set; }
    public int UserId { get; private set; }
    public string UserName { get; private set; } = "";
    public string Email { get; private set; } = "";
    public string Phone { get; private set; } = "";
    public bool IsAdmin { get; private set; }

    public event Action? OnChange;

    public UserStateService(IDbContextFactory<HomeNestDbContext> factory)
    {
        _factory = factory;
    }

    public void RestoreFromSession(UserSessionDto session)
    {
        IsLoggedIn = true;
        UserId = session.UserId;
        UserName = session.UserName;
        Email = session.Email;
        Phone = session.Phone;
        IsAdmin = session.IsAdmin;
        NotifyStateChanged();
    }

    public void RestoreFromClaims(System.Security.Claims.ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            return;

        IsLoggedIn = true;
        UserId = userId;
        UserName = user.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? "";
        Email = user.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "";
        Phone = user.FindFirst("Phone")?.Value ?? "";
        IsAdmin = bool.TryParse(user.FindFirst("IsAdmin")?.Value, out var isAdmin) && isAdmin;
        NotifyStateChanged();
    }

    public async Task<bool> RegisterAsync(string name, string email, string phone, string password)
    {
        await using var db = await _factory.CreateDbContextAsync();
        if (await db.Users.AnyAsync(u => u.Email == email))
            return false;

        var user = new User
        {
            Name = name,
            Email = email,
            Phone = phone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            IsAdmin = false
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return false;

        IsLoggedIn = true;
        UserId = user.Id;
        UserName = user.Name;
        Email = user.Email;
        Phone = user.Phone;
        IsAdmin = user.IsAdmin;

        NotifyStateChanged();
        return true;
    }

    public void Logout()
    {
        IsLoggedIn = false;
        UserId = 0;
        UserName = "";
        Email = "";
        Phone = "";
        IsAdmin = false;
        NotifyStateChanged();
    }

    public async Task ToggleFavoriteAsync(int propertyId)
    {
        if (!IsLoggedIn) return;

        await using var db = await _factory.CreateDbContextAsync();
        var existing = await db.Favorites
            .FirstOrDefaultAsync(f => f.UserId == UserId && f.PropertyId == propertyId);

        if (existing != null)
        {
            db.Favorites.Remove(existing);
        }
        else
        {
            db.Favorites.Add(new Favorite { UserId = UserId, PropertyId = propertyId });
        }

        await db.SaveChangesAsync();
        NotifyStateChanged();
    }

    public async Task<bool> IsFavoriteAsync(int propertyId)
    {
        if (!IsLoggedIn) return false;
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Favorites.AnyAsync(f => f.UserId == UserId && f.PropertyId == propertyId);
    }

    public async Task<List<int>> GetFavoriteIdsAsync()
    {
        if (!IsLoggedIn) return new List<int>();
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Favorites.AsNoTracking()
            .Where(f => f.UserId == UserId)
            .Select(f => f.PropertyId)
            .ToListAsync();
    }

    public async Task<List<Property>> GetFavoritesAsync()
    {
        if (!IsLoggedIn) return new List<Property>();
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Favorites.AsNoTracking()
            .Where(f => f.UserId == UserId)
            .Include(f => f.Property)
            .Select(f => f.Property)
            .ToListAsync();
    }

    public async Task<List<Property>> GetMyListingsAsync()
    {
        if (!IsLoggedIn) return new List<Property>();
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Properties.AsNoTracking()
            .Where(p => p.OwnerId == UserId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
