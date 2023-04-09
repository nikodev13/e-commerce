namespace ECommerce.ApplicationCore.Entities;

public class WishlistProduct
{
    public required Guid CustomerId { get; init; }
    public required long ProductId { get; init; }
    public Product Product { get; init; } = default!;
}