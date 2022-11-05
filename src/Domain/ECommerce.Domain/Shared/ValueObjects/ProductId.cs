namespace ECommerce.Domain.Shared.ValueObjects;

public class ProductId
{
    public ulong Value { get; }

    public ProductId(ulong value)
    {
        Value = value;
    }

    public static implicit operator ulong(ProductId id) => id.Value;
    public static implicit operator ProductId(ulong id) => new(id);
}