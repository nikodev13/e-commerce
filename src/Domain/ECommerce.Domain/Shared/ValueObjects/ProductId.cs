namespace ECommerce.Domain.Shared.ValueObjects;

public class ProductId
{
    public Guid Value { get; }

    public ProductId(Guid value)
    {
        Value = value;
    }

    public static implicit operator Guid(ProductId id) => id.Value;
    public static implicit operator ProductId(Guid id) => new(id);
}