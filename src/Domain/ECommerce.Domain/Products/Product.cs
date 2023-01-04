using ECommerce.Domain.Products.ValueObjects;
using ECommerce.Domain.Shared.Services;
using ECommerce.Domain.Shared.ValueObjects;

namespace ECommerce.Domain.Products;

public class Product
{
    public ProductId Id { get; }
    public required ProductName Name { get; set; }
    public required Description Description { get; set; }
    public required CategoryId CategoryId { get; set; }
    public required MoneyValue Price { get; set; }
    public required Quantity InStockQuantity { get; set; }

    private Product(ProductId id)
    {
        Id = id;
    }
    
    public static Product CreateNew(ProductName name,
        Description description, 
        CategoryId categoryId,
        MoneyValue price,
        Quantity quantity,
        ISnowflakeIdService idService)
    {
        return new Product(idService.GenerateId())
        {
            Name = name,
            Description = description,
            CategoryId = categoryId,
            Price = price,
            InStockQuantity = quantity
        };
    }
}