using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Management.ValueObjects;

public class ProductName
{
    public string Value { get; }

    public ProductName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Product name cannot be empty.");
        }
        Value = name;
    }

    public static implicit operator string(ProductName id) => id.Value;
    public static implicit operator ProductName(string name) => new(name);
}