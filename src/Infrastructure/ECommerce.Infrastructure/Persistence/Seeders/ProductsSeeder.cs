using Bogus;
using ECommerce.Application.Shared.Services;
using ECommerce.Domain.Products;
using ECommerce.Domain.ProductsContext.ValueObjects;

namespace ECommerce.Infrastructure.Persistence.Seeders;

internal static class ProductsSeeder
{
    private static ECommerceDbContext _dbContext;
    public static async Task SeedSampleProductsDataAsync(this ECommerceDbContext dbContext)
    {
        _dbContext = dbContext;
        if (!_dbContext.Categories.Any()) await SeedCategories();
        await _dbContext.SaveChangesAsync();
    }

    private static async Task SeedCategories()
    {
        var snowflakeService = new SnowflakeIdService();

        var fakeCategoryFactory = new Faker<Category>()
            .CustomInstantiator(x => Category.Create(new CategoryName(x.Commerce.Categories(1).First()), snowflakeService));

        var data = fakeCategoryFactory.Generate(10);
        await _dbContext.Categories.AddRangeAsync(data);
    }
}