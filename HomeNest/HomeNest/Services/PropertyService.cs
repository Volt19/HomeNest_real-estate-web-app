using HomeNest.Data;
using HomeNest.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeNest.Services;

public class SortOption
{
    public string Value { get; set; } = "";
    public string Label { get; set; } = "";
}

public class PropertyService
{
    private readonly IDbContextFactory<HomeNestDbContext> _factory;

    public PropertyService(IDbContextFactory<HomeNestDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<List<Property>> GetAllPropertiesAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Properties.AsNoTracking().OrderByDescending(p => p.CreatedAt).ToListAsync();
    }

    public async Task<Property?> GetPropertyByIdAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Properties.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    }

    public List<string> GetDistricts() => new()
    {
        "Всички", "Приморски", "Център", "Одесос", "Левски", "Владиславово", "Чайка", "Аспарухово"
    };

    public List<string> GetPropertyTypes() => new()
    {
        "Всички", "Студио", "2-стаен", "3-стаен", "Мезонет", "Къща", "Пентхаус", "Офис", "Ателие"
    };

    public List<SortOption> GetSortOptions() => new()
    {
        new SortOption { Value = "default", Label = "По подразбиране" },
        new SortOption { Value = "price-asc", Label = "Цена: Ниска към Висока" },
        new SortOption { Value = "price-desc", Label = "Цена: Висока към Ниска" },
        new SortOption { Value = "area-asc", Label = "Площ: Малка към Голяма" },
        new SortOption { Value = "area-desc", Label = "Площ: Голяма към Малка" }
    };

    public async Task<int> AddPropertyAsync(Property property)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Properties.Add(property);
        await db.SaveChangesAsync();
        return property.Id;
    }

    public async Task<bool> RemovePropertyAsync(int id, int? ownerId = null)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var property = await db.Properties.FindAsync(id);
        if (property == null) return false;
        if (ownerId.HasValue && property.OwnerId != ownerId.Value) return false;

        db.Properties.Remove(property);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdatePropertyAsync(Property updatedProperty, int? ownerId = null)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var existing = await db.Properties.FindAsync(updatedProperty.Id);
        if (existing == null) return false;
        if (ownerId.HasValue && existing.OwnerId != ownerId.Value) return false;

        existing.Title = updatedProperty.Title;
        existing.Description = updatedProperty.Description;
        existing.Price = updatedProperty.Price;
        existing.Area = updatedProperty.Area;
        existing.Rooms = updatedProperty.Rooms;
        existing.District = updatedProperty.District;
        existing.Type = updatedProperty.Type;
        existing.Furnished = updatedProperty.Furnished;
        existing.Floor = updatedProperty.Floor;
        existing.Image = updatedProperty.Image;
        existing.Features = updatedProperty.Features;
        existing.PriceUnit = updatedProperty.PriceUnit;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetPropertyCountAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Properties.CountAsync();
    }

    public async Task<List<Property>> GetPropertiesByOwnerAsync(int ownerId)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Properties.AsNoTracking()
            .Where(p => p.OwnerId == ownerId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Property>> GetSimilarPropertiesAsync(int propertyId, string district, string type, int count = 3)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Properties.AsNoTracking()
            .Where(p => p.Id != propertyId)
            .Where(p => p.District == district || p.Type == type)
            .Take(count)
            .ToListAsync();
    }
}
