namespace ECommerce.Domain.Shared.ValueObjects.Ids;

public class CustomerId
{
    public Guid Value { get; }

    public CustomerId(Guid value)
    {
        Value = value;
    }

    public static implicit operator Guid(CustomerId id) => id.Value;
    public static implicit operator CustomerId(Guid id) => new(id);
}