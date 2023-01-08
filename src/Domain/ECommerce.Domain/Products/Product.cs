using ECommerce.Domain.Products.ValueObjects;
using ECommerce.Domain.Shared.Services;
using ECommerce.Domain.Shared.ValueObjects;

namespace ECommerce.Domain.Products;

public class Product
{
    public required ProductId Id { get; init; }
    public required ProductName Name { get; set; }
    public required Description Description { get; set; }
    public required Category Category { get; set; }
    public required MoneyValue Price { get; set; }
    public required Quantity InStockQuantity { get; set; }

    private Product() { }
    
    public static Product CreateNew(ProductName name,
        Description description, 
        Category category,
        MoneyValue price,
        Quantity quantity,
        ISnowflakeIdService idService)
    {
        return new Product
        {
            Id = idService.GenerateId(),
            Name = name,
            Description = description,
            Category = category,
            Price = price,
            InStockQuantity = quantity
        };
    }
}