using ECommerce.ApplicationCore.Shared.Entities;

namespace ECommerce.ApplicationCore.Entities;

public class Product : AuditableEntity
{
    public required long Id { get; init; }
    public required string Name { get; set; }
    public required string? Description { get; set; }
    public required Category Category { get; set; }
    public required decimal Price { get; set; }
    public required uint InStockQuantity { get; set; }
    public required bool IsActive { get; set; }
}