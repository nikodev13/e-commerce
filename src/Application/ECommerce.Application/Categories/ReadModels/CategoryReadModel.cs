using ECommerce.Domain.Products;

namespace ECommerce.Application.Categories.ReadModels;

public class CategoryReadModel
{
    public long Id { get; init; }
    public string Name { get; init; }

    public static CategoryReadModel FromCategory(Category category)
    {
        return new CategoryReadModel()
        {
            Id = category.Id,
            Name = category.Name,
        };
    }
}

