using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Shared.ValueObjects;

public class Street
{
    public string Value { get; }

    public Street(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new DomainException("Address street can not be empty.");
        Value = value;
    }

}