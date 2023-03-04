using ECommerce.ApplicationCore.Shared.Entities;

namespace ECommerce.ApplicationCore.Entities;

public class Category : AuditableEntity
{
    public required long Id { get; init; }
    public required string Name { get; set; }
}