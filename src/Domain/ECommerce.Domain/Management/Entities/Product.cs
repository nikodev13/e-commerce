using ECommerce.Domain.Management.ValueObjects;
using ECommerce.Domain.Shared.Services;
using ECommerce.Domain.Shared.ValueObjects;
using ECommerce.Domain.Shared.ValueObjects.Ids;

namespace ECommerce.Domain.Management.Entities;

public class Product
{
    public required ProductId Id { get; init; }
    public required ProductName Name { get; set; }
    public required Description Description { get; set; }
    public required Category Category { get; set; }
    public required MoneyValue Price { get; set; }
    public required Quantity InStockQuantity { get; set; }
    public required bool IsActive { get; set; }

    private Product() { }
    
    public static Product CreateNew(ProductName name,
        Description description, 
        Category category,
        MoneyValue price,
        Quantity quantity,
        bool isActive,
        ISnowflakeIdProvider idProvider)
    {
        return new Product
        {
            Id = idProvider.GenerateId(),
            Name = name,
            Description = description,
            Category = category,
            Price = price,
            InStockQuantity = quantity,
            IsActive = isActive
        };
    }
}