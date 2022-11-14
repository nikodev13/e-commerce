namespace ECommerce.Domain.Products.Categories.ValueObjects;

public class CategoryId
{
    public uint Value { get; }

    public CategoryId(uint id)
    {
        Value = id;
    }

    public static implicit operator uint(CategoryId id) => id.Value;
    public static implicit operator CategoryId(uint id) => new(id);
}