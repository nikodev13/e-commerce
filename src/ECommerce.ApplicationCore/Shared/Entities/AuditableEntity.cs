namespace ECommerce.ApplicationCore.Shared.Entities;

public abstract class AuditableEntity
{
    public required Guid CreatedBy { get; init; }
    public required DateTime CreatedAt { get; init; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
}