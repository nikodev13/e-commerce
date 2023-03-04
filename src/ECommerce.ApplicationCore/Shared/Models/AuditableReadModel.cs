namespace ECommerce.ApplicationCore.Shared.Models;

public class AuditableReadModel
{
    public required Guid CreatedBy { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required Guid? LastModifiedBy { get; init; }
    public required DateTime? LastModifiedAt { get; init; }
}