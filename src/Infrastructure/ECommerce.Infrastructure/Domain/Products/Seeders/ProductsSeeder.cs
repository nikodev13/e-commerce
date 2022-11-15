using Bogus;
using ECommerce.Domain.Products;
using ECommerce.Domain.Products.Services;
using ECommerce.Domain.Products.ValueObjects;
using ECommerce.Domain.Shared.ValueObjects;
using ECommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Internal;

namespace ECommerce.Infrastructure.Domain.Products.Seeders;

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
        var fakeCategoryFactory = new Faker<Category>()
            .CustomInstantiator(x => (Category)Activator.CreateInstance(typeof(Category), true)!)
            .RuleFor(x => x.Name, x => new CategoryName(x.Commerce.Categories(1).First()));

        var data = fakeCategoryFactory.Generate(10);
        await _dbContext.Categories.AddRangeAsync(data);
    }
}