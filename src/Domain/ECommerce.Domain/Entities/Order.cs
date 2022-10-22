namespace ECommerce.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    
    public DateTime DateOfOrder { get; set; }
    public DateTime? DateOfDelivery { get; set; }

    public Guid ProductOfferId { get; set; }
    public ProductOffer ProductOffer { get; set; }

    public OrderStatus OrderStatus { get; set; } = OrderStatus.Created;
}