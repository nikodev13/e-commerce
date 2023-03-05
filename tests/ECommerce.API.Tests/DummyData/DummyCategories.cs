using ECommerce.ApplicationCore.Entities;
using ECommerce.Infrastructure.Services;

namespace ECommerce.API.Tests.DummyData;

public class DummyCategories
{
    public static List<Category> Data { get; }

    static DummyCategories()
    {
        Data = new List<Category>
        {
            new()
            {
                Id = 6016117943566336,
                Name = "First category",
                CreatedBy = DummyUsers.Data[0].Id,
                CreatedAt = DateTime.Now
            },
            new()
            {
                Id = 6016117943828480,
                Name = "Second category",
                CreatedBy = DummyUsers.Data[1].Id,
                CreatedAt = DateTime.Now
            }
        };
    }
}