using HomeNest.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeNest.Data;

public class HomeNestDbContext : DbContext
{
    public HomeNestDbContext(DbContextOptions<HomeNestDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<NewsletterSubscriber> NewsletterSubscribers => Set<NewsletterSubscriber>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Favorite>()
            .HasIndex(f => new { f.UserId, f.PropertyId })
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<NewsletterSubscriber>()
            .HasIndex(s => s.Email)
            .IsUnique();

        // Seed initial properties
        modelBuilder.Entity<Property>().HasData(
            new Property { Id = 1, Title = "Апартамент", Description = "Отдава се апартамент под наем в район Приморски, гр.Варна", Price = 550, Area = 80, Rooms = 3, District = "Приморски", Type = "3-стаен", Furnished = true, Floor = 3, Image = "images/property-offer-1.jpg", Features = "3 стаен,Обзаведен,80 кв.м", PriceUnit = "/ месец" },
            new Property { Id = 2, Title = "Студио", Description = "Уютно студио в центъра на Варна, близо до Морската градина", Price = 350, Area = 45, Rooms = 1, District = "Център", Type = "Студио", Furnished = true, Floor = 2, Image = "images/property-offer-2.jpg", Features = "Студио,Обзаведен,45 кв.м", PriceUnit = "/ месец" },
            new Property { Id = 3, Title = "Къща", Description = "Просторна семейна къща в тих квартал с градина", Price = 1200, Area = 180, Rooms = 5, District = "Владиславово", Type = "Къща", Furnished = false, Floor = 2, Image = "images/property-offer-3.jpg", Features = "5 стаен,Необзаведен,180 кв.м", PriceUnit = "/ месец" },
            new Property { Id = 4, Title = "Мезонет", Description = "Луксозен мезонет с панорамна гледка към морето", Price = 950, Area = 140, Rooms = 4, District = "Приморски", Type = "Мезонет", Furnished = true, Floor = 6, Image = "images/property-1.jpg", Features = "4 стаен,Обзаведен,140 кв.м", PriceUnit = "/ месец" },
            new Property { Id = 5, Title = "Двустаен", Description = "Приятен двустаен апартамент в квартал Левски", Price = 400, Area = 65, Rooms = 2, District = "Левски", Type = "2-стаен", Furnished = true, Floor = 4, Image = "images/property-2.jpg", Features = "2 стаен,Обзаведен,65 кв.м", PriceUnit = "/ месец" },
            new Property { Id = 6, Title = "Тристаен", Description = "Нов тристаен апартамент в модерен комплекс в Одесос", Price = 600, Area = 95, Rooms = 3, District = "Одесос", Type = "3-стаен", Furnished = false, Floor = 5, Image = "images/property-3.jpg", Features = "3 стаен,Необзаведен,95 кв.м", PriceUnit = "/ месец" },
            new Property { Id = 7, Title = "Офис", Description = "Офис площ в бизнес сграда в центъра на града", Price = 800, Area = 110, Rooms = 3, District = "Център", Type = "Офис", Furnished = true, Floor = 3, Image = "images/property-1.jpg", Features = "3 помещения,Обзаведен,110 кв.м", PriceUnit = "/ месец" },
            new Property { Id = 8, Title = "Ателие", Description = "Творческо ателие в арт зона на града", Price = 300, Area = 55, Rooms = 1, District = "Чайка", Type = "Ателие", Furnished = false, Floor = 1, Image = "images/property-2.jpg", Features = "Отворен план,Необзаведен,55 кв.м", PriceUnit = "/ месец" },
            new Property { Id = 9, Title = "Пентхаус", Description = "Роскошен пентхаус с тераса и барбекю зона", Price = 1500, Area = 160, Rooms = 4, District = "Приморски", Type = "Пентхаус", Furnished = true, Floor = 8, Image = "images/property-3.jpg", Features = "4 стаен,Обзаведен,160 кв.м", PriceUnit = "/ месец" },
            new Property { Id = 10, Title = "Двустаен", Description = "Компактен двустаен в близост до университета", Price = 380, Area = 58, Rooms = 2, District = "Одесос", Type = "2-стаен", Furnished = true, Floor = 2, Image = "images/property-1.jpg", Features = "2 стаен,Обзаведен,58 кв.м", PriceUnit = "/ месец" },
            new Property { Id = 11, Title = "Тристаен", Description = "Панорамен тристаен с изглед към Варненското езеро", Price = 700, Area = 105, Rooms = 3, District = "Аспарухово", Type = "3-стаен", Furnished = true, Floor = 7, Image = "images/property-2.jpg", Features = "3 стаен,Обзаведен,105 кв.м", PriceUnit = "/ месец" },
            new Property { Id = 12, Title = "Къща", Description = "Нова къща в затворен комплекс с охрана", Price = 1800, Area = 220, Rooms = 6, District = "Владиславово", Type = "Къща", Furnished = false, Floor = 2, Image = "images/property-3.jpg", Features = "6 стаен,Необзаведен,220 кв.м", PriceUnit = "/ месец" }
        );
		modelBuilder.Entity<User>().HasData(


	   );
	}
}
