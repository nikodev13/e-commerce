using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Shared.ValueObjects;

public class PhoneNumber
{
    public string Value { get; }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidPhoneNumberException();

        Value = value;
    }
}

public class InvalidPhoneNumberException : DomainException
{
    public InvalidPhoneNumberException() : base("Phone number can not be empty.") { }
}