using ECommerce.Domain.ProductsContext;
using ECommerce.Domain.ProductsContext.ValueObjects;
using ECommerce.Domain.SeedWork;
using ECommerce.Domain.Shared.ValueObjects;

namespace ECommerce.Domain.Products;

public class Product : Entity
{
    public ProductId Id { get; init; }
    public ProductName Name { get; set; }
    public Description Description { get; set; }
    public Category Category { get; set; }

    private readonly List<ProductOffer> _productOffers;
    public IEnumerable<ProductOffer> ProductOffers => _productOffers;

    public Product()
    {
        _productOffers = new List<ProductOffer>();
    }

    public void MakeOffer(MoneyValue price, Quantity quantity)
    {
        var offer = new ProductOffer(Id, price, quantity);
        _productOffers.Add(offer);
    }
}