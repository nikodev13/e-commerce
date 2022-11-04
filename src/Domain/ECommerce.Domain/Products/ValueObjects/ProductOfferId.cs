namespace ECommerce.Domain.Products.ValueObjects;

public class ProductOfferId
{
    public ulong Value { get; }

    public ProductOfferId(ulong value)
    {
        Value = value;
    }

    public static implicit operator ulong(ProductOfferId id) => id.Value;
    public static implicit operator ProductOfferId(ulong id) => new(id);
}