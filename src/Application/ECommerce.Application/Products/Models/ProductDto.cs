using ECommerce.Domain.Products;

namespace ECommerce.Application.Products.Models;

public class ProductDto
{
    public long Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string Category { get; init; }

    public static ProductDto FromProduct(Product product)
    {
        return new ProductDto()
        {
            Id = product.Id.Value,
            Name = product.Name.Value,
            Description = product.Description.Value,
            Category = product.Category.Name.Value
        };
    }
}