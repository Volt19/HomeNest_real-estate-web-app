using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HomeNest.Data;

public class HomeNestDbContextFactory : IDesignTimeDbContextFactory<HomeNestDbContext>
{
    public HomeNestDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<HomeNestDbContext>();
        optionsBuilder.UseSqlite("Data Source=homenest.db")
            .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        return new HomeNestDbContext(optionsBuilder.Options);
    }
}
