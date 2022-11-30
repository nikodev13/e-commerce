using Bogus;
using ECommerce.Domain.Products;
using ECommerce.Domain.Products.ValueObjects;
using ECommerce.Domain.Shared.Services;
using ECommerce.Domain.Shared.ValueObjects;

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
        var fakeCategoryFactory = new Faker<Category>()
            .CustomInstantiator(x => new Category(x.UniqueIndex, x.Commerce.Categories(1).First()));

        var data = fakeCategoryFactory.Generate(10);
        return data;
    }
}