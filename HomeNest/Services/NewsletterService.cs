using HomeNest.Data;
using HomeNest.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeNest.Services;

public class NewsletterService
{
    private readonly IDbContextFactory<HomeNestDbContext> _factory;

    public NewsletterService(IDbContextFactory<HomeNestDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<bool> SubscribeAsync(string email)
    {
        email = email.Trim().ToLowerInvariant();
        if (!IsValidEmail(email)) return false;

        await using var db = await _factory.CreateDbContextAsync();
        if (await db.NewsletterSubscribers.AnyAsync(s => s.Email == email))
            return false;

        db.NewsletterSubscribers.Add(new NewsletterSubscriber { Email = email });
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<List<NewsletterSubscriber>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.NewsletterSubscribers.AsNoTracking()
            .OrderByDescending(s => s.SubscribedAt)
            .ToListAsync();
    }

    public async Task<int> GetCountAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.NewsletterSubscribers.CountAsync();
    }

    public async Task<bool> IsSubscribedAsync(string email)
    {
        email = email.Trim().ToLowerInvariant();
        await using var db = await _factory.CreateDbContextAsync();
        return await db.NewsletterSubscribers.AnyAsync(s => s.Email == email);
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
