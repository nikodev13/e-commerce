using ECommerce.Domain.Products.ValueObjects;
using ECommerce.Domain.Shared.Services;
using ECommerce.Domain.Shared.ValueObjects;

namespace ECommerce.Domain.Products;

public class Category
{
    public required CategoryId Id { get; init; }
    public required string Name { get; set; }

    private Category() { }

    public static Category CreateNew(string name, ISnowflakeIdService idService)
    {
        return new Category
        {
            Id = idService.GenerateId(),
            Name = name
        };
    }
}