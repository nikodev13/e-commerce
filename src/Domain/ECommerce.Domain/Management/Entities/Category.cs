using ECommerce.Domain.Shared.Services;
using ECommerce.Domain.Shared.ValueObjects;
using ECommerce.Domain.Shared.ValueObjects.Ids;

namespace ECommerce.Domain.Management.Entities;

public class Category
{
    public required CategoryId Id { get; init; }
    public required string Name { get; set; }

    private Category() { }

    public static Category CreateNew(string name, ISnowflakeIdProvider idProvider)
    {
        return new Category
        {
            Id = idProvider.GenerateId(),
            Name = name
        };
    }
}