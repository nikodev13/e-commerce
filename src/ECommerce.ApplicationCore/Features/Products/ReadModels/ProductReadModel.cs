using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.Products.ReadModels;

public record ProductReadModel(long Id, string Name, string Description, string Category, decimal Price, uint Quantity, bool IsActive)
{
    public static ProductReadModel From(Product product)
        => new(product.Id,
            product.Name,
            product.Description ?? string.Empty,
            product.Category.Name,
            product.Price,
            product.InStockQuantity,
            product.IsActive);
}