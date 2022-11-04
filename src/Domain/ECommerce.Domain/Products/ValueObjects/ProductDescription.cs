
namespace ECommerce.Domain.Products.ValueObjects;

public class ProductDescription
{
    public string Value { get; }

    public ProductDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description.Trim()))
            throw new InvalidProductDescriptionException();
        Value = description;
    }
    
    public static implicit operator string(ProductDescription description) => description.Value;
    public static implicit operator ProductDescription(string description) => new(description);
}