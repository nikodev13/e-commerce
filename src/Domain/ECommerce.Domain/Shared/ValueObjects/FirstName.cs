using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Shared.ValueObjects;

public class FirstName
{
    public string Value { get; }

    public FirstName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidFirstNameException();

        Value = value;
    }
}

public class InvalidFirstNameException : DomainException
{
    public InvalidFirstNameException() : base("Customer first name can not be empty.") { }
}