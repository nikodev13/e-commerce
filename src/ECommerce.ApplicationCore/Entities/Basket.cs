namespace ECommerce.ApplicationCore.Entities;

public class Basket
{
    public required Guid CustomerId { get; init; }
    public List<BasketItem> Items { get; } = new();
    public DateTime? LastModified { get; set; }
}

public class BasketItem
{
    public required long Id { get; init; }
    public required long ProductId { get; init; }
    public required uint Quantity { get; init; }
}