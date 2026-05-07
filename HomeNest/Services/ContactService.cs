using HomeNest.Data;
using HomeNest.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeNest.Services;

public class ContactService
{
    private readonly IDbContextFactory<HomeNestDbContext> _factory;

    public ContactService(IDbContextFactory<HomeNestDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<List<ContactMessage>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.ContactMessages.AsNoTracking()
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<ContactMessage?> GetByIdAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.ContactMessages.FindAsync(id);
    }

    public async Task AddAsync(ContactMessage message)
    {
        if (string.IsNullOrWhiteSpace(message.Name) || string.IsNullOrWhiteSpace(message.Email) || string.IsNullOrWhiteSpace(message.Message))
            throw new ArgumentException("Name, Email and Message are required.");

        await using var db = await _factory.CreateDbContextAsync();
        db.ContactMessages.Add(message);
        await db.SaveChangesAsync();
    }

    public async Task MarkAsReadAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var msg = await db.ContactMessages.FindAsync(id);
        if (msg != null)
        {
            msg.IsRead = true;
            await db.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var msg = await db.ContactMessages.FindAsync(id);
        if (msg != null)
        {
            db.ContactMessages.Remove(msg);
            await db.SaveChangesAsync();
        }
    }

    public async Task<int> GetUnreadCountAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.ContactMessages.CountAsync(m => !m.IsRead);
    }
}
