namespace ECommerce.Domain.Shared.ValueObjects.Ids;

public class ProductId
{
    public long Value { get; }

    public ProductId(long value)
    {
        Value = value;
    }

    public static implicit operator long(ProductId id) => id.Value;
    public static implicit operator ProductId(long id) => new(id);
}