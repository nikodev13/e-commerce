using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Customers.ValueObjects;

public class LastName
{
    public string Value { get; }

    public LastName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Customer last name can not be empty.");

        Value = value;
    }
}