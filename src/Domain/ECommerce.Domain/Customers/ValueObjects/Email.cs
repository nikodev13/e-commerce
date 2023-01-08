using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Customers.ValueObjects;

public class Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email can not be empty.");

        Value = value;
    }
}