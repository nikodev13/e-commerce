using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Tests.Management.Categories;
using ECommerce.Infrastructure.Services.Services;

namespace ECommerce.Application.Tests.Management.Products;

public static class DummyProducts
{
    public static List<Product> Data { get; }

    static DummyProducts()
    {
        var idProvider = new SnowflakeIdProvider();

        Data = new List<Product>()
        {
            new()
            {
                Id = idProvider.GenerateId(),
                Name = "GTA V",
                Description = "Where's GTA VI ?",
                Category = DummyCategories.Data[0],
                Price = 300,
                InStockQuantity = 0,
                IsActive = false
            },
            new()
            {
                Id = idProvider.GenerateId(),
                Name = "Acer Nitro 5",
                Description = "Super duper nitro",
                Category = DummyCategories.Data[1],
                Price = 3000,
                InStockQuantity = 100,
                IsActive = false
            }
        };
    }
}