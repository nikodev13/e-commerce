using ECommerce.Domain.Products;

namespace ECommerce.Application.Products.ReadModels;

public class ProductReadModel
{
    public required long Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public required decimal Price { get; init; }
    public required uint Quantity { get; init; }

    public static ProductReadModel FromProduct(Product product)
    {
        var readModel = new ProductReadModel
        {
            Id = product.Id.Value,
            Name = product.Name.Value,
            Description = product.Description.Value,
            Category = product.Category.Name.Value,
            Price = product.Price.Value,
            Quantity = product.Quantity
        };
        
        return readModel;
    }
}