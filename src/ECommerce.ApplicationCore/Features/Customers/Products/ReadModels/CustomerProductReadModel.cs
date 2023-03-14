using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.Customers.Products.ReadModels;

public class CustomerProductReadModel
{
    public required long Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public required decimal Price { get; init; }
    public required uint Quantity { get; init; }

    private CustomerProductReadModel() { }

    public static CustomerProductReadModel From(Product product)
    {
        var readModel = new CustomerProductReadModel()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description ?? string.Empty,
            Category = product.Category.Name,
            Price = product.Price,
            Quantity = product.InStockQuantity,
        };

        return readModel;
    }
}