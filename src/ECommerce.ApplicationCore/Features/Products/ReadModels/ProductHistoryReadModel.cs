namespace ECommerce.ApplicationCore.Features.Products.ReadModels;

public record ProductHistoryReadModel(long ProductId, Guid? LastModifiedBy, DateTime? LastModifiedAt, Guid CreatedBy,
    DateTime CreatedAt);
