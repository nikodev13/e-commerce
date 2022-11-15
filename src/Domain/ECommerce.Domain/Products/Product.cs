using System.Collections.ObjectModel;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Products.ValueObjects;
using ECommerce.Domain.Shared.ValueObjects;

namespace ECommerce.Domain.Products;

public class Product
{
    public ProductId Id { get; }
    public ProductName Name { get; set; }
    public Description Description { get; set; }
    public Category Category { get; set; }

    private readonly List<ProductOffer> _productOffers;
    public IEnumerable<ProductOffer> ProductOffers => _productOffers;

    private Product()
    {
        _productOffers = new List<ProductOffer>();
    }

    internal Product(ProductName name, Description description, Category category)
    {
        Name = name;
        Description = description;
        Category = category;
    }
    
    public void MakeOffer(MoneyValue price, Quantity quantity)
    {
        var offer = new ProductOffer(Id, price, quantity);
        _productOffers.Add(offer);
    }
}