using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Database;

public class ECommerceDatabaseSeeder : IDatabaseSeeder<ECommerceDbContext>
{
    private readonly DbContextOptions<ECommerceDbContext> _options;

    public ECommerceDatabaseSeeder(Action<DbContextOptionsBuilder<ECommerceDbContext>> optionsAction)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ECommerceDbContext>();
        optionsAction.Invoke(optionsBuilder);
        _options = optionsBuilder.Options;
    }
    
    public void Seed()
    {
        using var dbContext = new ECommerceDbContext(_options);
        SeedDataFromAssemblies(dbContext);
    }

    private void SeedDataFromAssemblies(ECommerceDbContext dbContext)
    {
        var currentAssembly = Assembly.GetExecutingAssembly().GetTypes();

        
        var currentAssemblya = Assembly.GetExecutingAssembly();


    }
}