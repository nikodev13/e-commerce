namespace ECommerce.Domain.Customers.ValueObjects;

public class AddressId
{
    public long Value { get; }

    public AddressId(long value)
    {
        Value = value;
    }

    public static implicit operator long(AddressId id) => id.Value;
    public static implicit operator AddressId(long id) => new(id);
}