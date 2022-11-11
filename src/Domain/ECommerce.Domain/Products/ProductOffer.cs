using System.Net;
using ECommerce.Domain.Products;
using ECommerce.Domain.Products.ValueObjects;
using ECommerce.Domain.Shared.ValueObjects;

namespace ECommerce.Domain.Entities;

public class ProductOffer
{
    public ProductOfferId Id { get; }
    public ProductId ProductId { get; }
    public MoneyValue Price { get; }
    public Quantity Quantity { get; set; }
    public DateTime DateTime { get; }

    private ProductOffer()
    {
    }
    
    internal ProductOffer(ProductId productId, MoneyValue price, Quantity quantity)
    {
        ProductId = productId;
        Price = price;
        Quantity = quantity;
        DateTime = DateTime.Now;
    }
}