using ECommerce.Domain.Products;
using ECommerce.Domain.ProductsContext;

namespace ECommerce.Domain.Entities;

public class CartItem
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}