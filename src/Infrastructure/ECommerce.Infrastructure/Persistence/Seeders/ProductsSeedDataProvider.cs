using Bogus;
using ECommerce.Application.Common.Services;
using ECommerce.Domain.Products;
using ECommerce.Domain.Shared.Services;

namespace ECommerce.Infrastructure.Persistence.Seeders;

internal static class ProductsSeedDataProvider
{
    private static ISnowflakeIdService _idService;
    private static ApplicationDbContext _dbContext;

    public static void SeedProductContextSampleData(this ApplicationDbContext dbContext)
    {
        _idService = new SnowflakeIdService();
        _dbContext = dbContext;
        if (!dbContext.Categories.Any())
        {
            var data = GetCategoriesData();
            dbContext.Categories.AddRange(data);
            dbContext.SaveChanges();
        }
        if (!dbContext.Products.Any())
        {
            var data = GetProductsData();
            dbContext.Products.AddRange(data);
            dbContext.SaveChanges();
        }
    }
    
    private static IEnumerable<Category> GetCategoriesData()
    {
        var fakeCategoryFactory = new Faker<Category>()
            .CustomInstantiator(x => Category.CreateNew(x.Commerce.Categories(1).First(), _idService));
    
        var data = fakeCategoryFactory.Generate(10);
        return data;
    }
    
    private static IEnumerable<Product> GetProductsData()
    {
        var categories = _dbContext.Categories.ToList();
    
        var fakeProductFactory = new Faker<Product>()
            .CustomInstantiator(x => Product.CreateNew(
                x.Commerce.ProductName(),
                x.Commerce.ProductDescription(),
                categories[Random.Shared.Next(0, categories.Count)],
                decimal.Parse(x.Commerce.Price()),
                x.Commerce.Random.Int(0, 100),
                _idService));
        var data = fakeProductFactory.Generate(10);
        return data;
    }
}