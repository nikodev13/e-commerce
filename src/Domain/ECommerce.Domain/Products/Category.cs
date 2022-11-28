using ECommerce.Domain.Products.ValueObjects;
using ECommerce.Domain.SeedWork;
using ECommerce.Domain.Shared.Services;
using ECommerce.Domain.Shared.ValueObjects;

namespace ECommerce.Domain.Products;

public class Category : Entity
{
    public CategoryId Id { get; }
    public CategoryName Name { get; set; }

    public Category(CategoryId id, CategoryName name)
    {
        Id = id;
        Name = name;
    }
}