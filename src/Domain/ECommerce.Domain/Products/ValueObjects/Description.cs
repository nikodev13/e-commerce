using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Products.ValueObjects;

public class Description
{
    public string Value { get; }

    public Description(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Description cannot be empty.");
        }
        Value = description;
    }

    public static implicit operator string(Description description) => description.Value;
    public static implicit operator Description(string description) => new(description);
}