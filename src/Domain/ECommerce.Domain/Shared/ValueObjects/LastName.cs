using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Shared.ValueObjects;

public class LastName
{
    public string Value { get; }

    public LastName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidLastNameException();

        Value = value;
    }
}

public class InvalidLastNameException : DomainException
{
    public InvalidLastNameException() : base("Customer last name can not be empty.") { }
}