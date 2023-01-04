using Bogus;
using ECommerce.Application.Common.Services;
using ECommerce.Domain.Products;
using ECommerce.Domain.Products.ValueObjects;
using ECommerce.Domain.ProductsContext.ValueObjects;
using ECommerce.Domain.Shared.Services;
using ECommerce.Domain.Shared.ValueObjects;

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
            .CustomInstantiator(x => new Category(_idService.GenerateId(), x.Commerce.Categories(1).First()));

        var data = fakeCategoryFactory.Generate(10);
        return data;
    }
    
    private static IEnumerable<Product> GetProductsData()
    {
        var categoryIds = _dbContext.Categories.Select(x => x.Id).ToList();

        var fakeProductFactory = new Faker<Product>()
            .CustomInstantiator(x => new Product() { Id = _idService.GenerateId() })
            .RuleFor(x => x.Name, x => new ProductName(x.Commerce.ProductName()))
            .RuleFor(x => x.Description, x => new Description(x.Commerce.ProductDescription()))
            .RuleFor(x => x.CategoryId,
                x => new CategoryId(categoryIds[Random.Shared.Next(0, categoryIds.Count)]));

        var data = fakeProductFactory.Generate(10);
        return data;
    }
}