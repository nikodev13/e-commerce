using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Products.ValueObjects;

public class CategoryName
{
    public string Value { get; }

    public CategoryName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Product name cannot be empty.");
        }
        Value = name;
    }

    public static implicit operator string(CategoryName name) => name.Value;
    public static implicit operator CategoryName(string name) => new(name);
}