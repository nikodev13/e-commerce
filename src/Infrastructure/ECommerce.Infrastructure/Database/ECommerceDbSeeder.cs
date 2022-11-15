using ECommerce.Infrastructure.Domain.Products.Seeders;

namespace ECommerce.Infrastructure.Database;

public class ECommerceDbSeeder
{
    private readonly ECommerceDbContext _dbContext;

    public ECommerceDbSeeder(ECommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SeedSampleData()
    {
        await _dbContext.SeedSampleProductsDataAsync();
    }    
}