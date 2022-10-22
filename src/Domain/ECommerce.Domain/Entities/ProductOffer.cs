namespace ECommerce.Domain.Entities;

public class ProductOffer
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime DateTime { get; set; }
}