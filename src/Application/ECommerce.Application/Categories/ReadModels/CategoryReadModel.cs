using ECommerce.Domain.Products;

namespace ECommerce.Application.Categories.ReadModels;

public class CategoryReadModel
{
    public required long Id { get; init; }
    public required string Name { get; init; }

    public static CategoryReadModel FromCategory(Category category)
    {
        return new CategoryReadModel
        {
            Id = category.Id,
            Name = category.Name,
        };
    }
}

