using ECommerce.Domain.Products.Categories.Exceptions;
using ECommerce.Domain.Products.Categories.Services;
using ECommerce.Domain.Shared.Services;
using ECommerce.Domain.Shared.ValueObjects;

namespace ECommerce.Domain.Products.Categories;

public class Category
{
    public CategoryId Id { get; private init; }
    public CategoryName Name { get; private set; }

    public void ChangeName(CategoryName name, ICategoryUniquenessChecker uniquenessChecker)
    {
        if (uniquenessChecker.IsNotUnique(name))
        {
            throw new CategoryAlreadyExistsException(name);
        }
        
        Name = name;
    }

    private Category()
    {
    }

    public static Category Create(CategoryName name,
        ISnowflakeIdService idService,
        ICategoryUniquenessChecker uniquenessChecker)
    {
        if (uniquenessChecker.IsNotUnique(name))
        {
            throw new CategoryAlreadyExistsException(name);
        }
        
        return new Category()
        {
            Id = idService.GenerateId(),
            Name = name,
        };
    }
}