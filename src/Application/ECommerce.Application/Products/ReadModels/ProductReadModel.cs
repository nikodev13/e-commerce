using ECommerce.Domain.Products;

namespace ECommerce.Application.Products.ReadModels;

public class ProductReadModel
{
    public long Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public string Category { get; init; }

    public static ProductReadModel FromProduct(Product product)
    {
        var readModel = new ProductReadModel()
        {
            Id = product.Id.Value,
            Name = product.Name.Value,
            Description = product.Description.Value,
            Category = product.Category.Name.Value
        };
        
        return readModel;
    }
}