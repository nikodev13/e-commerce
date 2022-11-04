using ECommerce.Domain.Products.Exceptions;

namespace ECommerce.Domain.Products.ValueObjects;

public class ProductName
{
    public string Value { get; }

    public ProductName(string name)
    {
        if (string.IsNullOrWhiteSpace(name.Trim()))
            throw new InvalidProductNameException();
        Value = name;
    }
    
    public static implicit operator string(ProductName name) => name.Value;
    public static implicit operator ProductName(string name) => new(name);
}