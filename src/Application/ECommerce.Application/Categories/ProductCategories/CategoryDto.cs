using ECommerce.Domain.Products;

namespace ECommerce.Application.Categories.ProductCategories;

public class CategoryDto
{
    public long Id { get; set; }
    public string Name { get; set; }

    public static CategoryDto FromCategory(Category category)
    {
        return new CategoryDto()
        {
            Id = category.Id,
            Name = category.Name,
        };
    }
}

