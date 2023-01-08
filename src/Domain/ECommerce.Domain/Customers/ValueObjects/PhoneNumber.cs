using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Customers.ValueObjects;

public class PhoneNumber
{
    public string Value { get; }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Phone number can not be empty.");

        Value = value;
    }
}