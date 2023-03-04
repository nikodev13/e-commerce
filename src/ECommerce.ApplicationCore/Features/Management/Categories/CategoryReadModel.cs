using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Shared.Entities;

namespace ECommerce.ApplicationCore.Features.Management.Categories;

public class CategoryReadModel : AuditableEntity
{
    public required long Id { get; init; }
    public required string Name { get; init; }

    public static CategoryReadModel FromCategory(Category category)
    {
        return new CategoryReadModel
        {
            Id = category.Id,
            Name = category.Name,
            CreatedBy = category.CreatedBy,
            CreatedAt = category.CreatedAt,
            LastModifiedBy = category.LastModifiedBy,
            LastModifiedAt = category.LastModifiedAt
        };
    }
}

