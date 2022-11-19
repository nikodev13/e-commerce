using ECommerce.Domain.ProductsContext.ValueObjects;
using ECommerce.Domain.Shared.Services;
using ECommerce.Domain.Shared.ValueObjects;

namespace ECommerce.Domain.Products;

public class Category
{
    public CategoryId Id { get; private init; }
    public CategoryName Name { get; set; }

    private Category()
    {
    }
    
    public static Category Create(CategoryName name, ISnowflakeIdService idService)
    {
        return new Category
        {
            Id = idService.GenerateId(),
            Name = name
        };
    }
}