using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Shared.ValueObjects;

public class Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidEmailException();

        Value = value.ToLower();
    }
}

public class InvalidEmailException : DomainException
{
    public InvalidEmailException() : base("Email can not be empty.") { }
}