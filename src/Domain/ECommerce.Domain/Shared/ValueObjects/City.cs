using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Shared.ValueObjects;

public class City
{
    public string Value { get; }

    public City(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new DomainException("Address postal code can not be empty.");
        Value = value;
    }
}