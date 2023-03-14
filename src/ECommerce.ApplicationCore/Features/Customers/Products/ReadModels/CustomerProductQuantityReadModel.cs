namespace ECommerce.ApplicationCore.Features.Customers.Products.ReadModels;

public class CustomerProductQuantityReadModel
{
    public required long ProductId { get; set; }
    public required uint InStockQuantity { get; init; }
}