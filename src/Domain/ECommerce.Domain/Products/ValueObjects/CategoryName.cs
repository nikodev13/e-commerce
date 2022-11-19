namespace ECommerce.Domain.ProductsContext.ValueObjects;

public class CategoryName
{
    public string Value { get; }

    public CategoryName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name cannot be empty.");
        }
        Value = name;
    }

    public static implicit operator string(CategoryName name) => name.Value;
    public static implicit operator CategoryName(string name) => new(name);
}