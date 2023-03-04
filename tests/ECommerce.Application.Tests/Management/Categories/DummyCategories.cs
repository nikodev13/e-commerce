using ECommerce.ApplicationCore.Entities;
using ECommerce.Infrastructure.Services.Services;

namespace ECommerce.Application.Tests.Management.Categories;

public static class DummyCategories
{
    public static List<Category> Data { get; }

    static DummyCategories()
    {
        var idProvider = new SnowflakeIdProvider();

        Data = new List<Category>
        {
            new()
            {
                Id = idProvider.GenerateId(),
                Name = "Games"
            },
            new()
            {
                Id = idProvider.GenerateId(),
                Name = "Laptops"
            },
            new()
            {
                Id = idProvider.GenerateId(),
                Name = "Has no products"
            }
        };
    }
}