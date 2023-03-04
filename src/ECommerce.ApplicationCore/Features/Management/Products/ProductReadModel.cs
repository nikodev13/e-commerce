using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Shared.Models;

namespace ECommerce.ApplicationCore.Features.Management.Products;

public class ProductReadModel : AuditableReadModel
{
    public required long Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public required decimal Price { get; init; }
    public required uint Quantity { get; init; }
    
    private ProductReadModel() { }
    
    public static ProductReadModel FromProduct(Product product)
    {
        var readModel = new ProductReadModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description ?? string.Empty,
            Category = product.Category.Name,
            Price = product.Price,
            Quantity = product.InStockQuantity,
            CreatedBy = product.CreatedBy,
            CreatedAt = product.CreatedAt,
            LastModifiedBy = product.LastModifiedBy,
            LastModifiedAt = product.LastModifiedAt
        };
        
        return readModel;
    }
}