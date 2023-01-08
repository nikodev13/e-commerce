using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Customers.ValueObjects;

public class FirstName
{
    public string Value { get; }

    public FirstName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Customer first name can not be empty.");

        Value = value;
    }
}