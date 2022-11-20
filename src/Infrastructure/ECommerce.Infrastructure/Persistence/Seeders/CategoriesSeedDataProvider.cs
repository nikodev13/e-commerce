using Bogus;
using ECommerce.Application.Shared.Services;
using ECommerce.Domain.Products;
using ECommerce.Domain.Products.Categories;
using ECommerce.Domain.Products.Categories.Services;
using ECommerce.Domain.ProductsContext.ValueObjects;
using ECommerce.Domain.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Seeders;

internal class CategoriesSeedDataProvider : IEntitySeedDataProvider<Category>
{
    private readonly ISnowflakeIdService _idService;

    public CategoriesSeedDataProvider(ISnowflakeIdService idService)
    {
        _idService = idService;
    }
    
    public IEnumerable<Category> GetData()
    {
        var uniquenessChecker = new CategoryAlwaysUniqueService();
        var fakeCategoryFactory = new Faker<Category>()
            .CustomInstantiator(x => Category.Create(new CategoryName(x.Commerce.Categories(1).First()),
                _idService,
                uniquenessChecker));

        var data = fakeCategoryFactory.Generate(10);
        return data;
    }
    
    private class CategoryAlwaysUniqueService : ICategoryUniquenessChecker
    {
        public bool IsNotUnique(CategoryName name)
        {
            return true;
        }
    }
}