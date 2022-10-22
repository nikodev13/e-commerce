namespace ECommerce.Domain.Entities;

public class Cart
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public ICollection<CartItem> Items { get; set; }
}