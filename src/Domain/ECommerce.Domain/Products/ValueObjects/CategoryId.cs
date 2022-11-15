namespace ECommerce.Domain.Products.ValueObjects;

public class CategoryId
{
    public Guid Value { get; }

    public CategoryId(Guid id)
    {
        Value = id;
    }

    public static implicit operator Guid(CategoryId id) => id.Value;
    public static implicit operator CategoryId(Guid id) => new(id);
}