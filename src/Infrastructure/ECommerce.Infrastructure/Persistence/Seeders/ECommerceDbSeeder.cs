using ECommerce.Domain.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Seeders;

public interface IEntitySeedDataProvider<out TEntity>
{
    IEnumerable<TEntity> GetData();
}

public class ECommerceDbSeeder
{
    private readonly ECommerceDbContext _dbContext;
    private readonly ISnowflakeIdService _idService;

    public ECommerceDbSeeder(ECommerceDbContext dbContext, ISnowflakeIdService idService)
    {
        _dbContext = dbContext;
        _idService = idService;
    }

    private async Task SeedCategories()
    {
        if (!await _dbContext.Categories.AnyAsync())
        {
            var categoriesSeedData = new CategoriesSeedDataProvider(_idService).GetData();
            await _dbContext.Categories.AddRangeAsync(categoriesSeedData);
        }
    }
    
    public async Task SeedSampleData()
    {
        await SeedCategories();
    }    
}