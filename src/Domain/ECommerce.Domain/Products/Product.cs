using System.Collections.ObjectModel;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Products.ValueObjects;

namespace ECommerce.Domain.Products;

public class Product
{
    public ProductId Id { get; }
    public string Name { get; }
    public string Description { get; }

    private readonly List<ProductOffer> _productOffers;
    public IReadOnlyCollection<ProductOffer> ProductOffers => _productOffers;
    
    private Product(string name, string description)
    {
        Name = name;
        Description = description;
    }
    
    public void MakeOffer(decimal price, uint quantity)
    {
        var offer = new ProductOffer(Id, price, quantity);
        _productOffers.Add(offer);
    }
}