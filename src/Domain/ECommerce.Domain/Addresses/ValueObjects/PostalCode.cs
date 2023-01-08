using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Addresses.ValueObjects;

public class PostalCode
{
    public string Value { get; }

    public PostalCode(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new DomainException("Address postal code can not be empty.");
        Value = value;
    }
}