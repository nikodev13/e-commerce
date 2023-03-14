using ECommerce.ApplicationCore.Entities;
using ECommerce.Infrastructure.Services;

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
                Name = "Games",
                CreatedBy = Guid.Parse("3DBD0163-906C-4413-8D48-23AEAC703B26"),
                CreatedAt = DateTime.Now
            },
            new()
            {
                Id = idProvider.GenerateId(),
                Name = "Laptops",
                CreatedBy = Guid.Parse("3DBD0163-906C-4413-8D48-23AEAC703B26"),
                CreatedAt = DateTime.Now
            },
            new()
            {
                Id = idProvider.GenerateId(),
                Name = "Has no products",
                CreatedBy = Guid.Parse("3DBD0163-906C-4413-8D48-23AEAC703B26"),
                CreatedAt = DateTime.Now
            }
        };
    }
}