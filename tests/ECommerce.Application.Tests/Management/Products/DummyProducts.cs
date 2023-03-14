using ECommerce.Application.Tests.Management.Categories;
using ECommerce.ApplicationCore.Entities;
using ECommerce.Infrastructure.Services;

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
                CategoryId = DummyCategories.Data[0].Id,
                Price = 300,
                InStockQuantity = 0,
                IsActive = false,
                CreatedBy = Guid.Parse("3DBD0163-906C-4413-8D48-23AEAC703B26"),
                CreatedAt = DateTime.Now,
            },
            new()
            {
                Id = idProvider.GenerateId(),
                Name = "Acer Nitro 5",
                Description = "Super duper nitro",
                CategoryId = DummyCategories.Data[1].Id,
                Price = 3000,
                InStockQuantity = 100,
                IsActive = false,
                CreatedBy = Guid.Parse("3DBD0163-906C-4413-8D48-23AEAC703B26"),
                CreatedAt = DateTime.Now,
            }
        };
    }
}