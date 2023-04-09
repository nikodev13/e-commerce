namespace ECommerce.ApplicationCore.Features.Categories.ReadModels;

public record CategoryHistoryReadModel(long CategoryId, Guid? LastModifiedBy, DateTime? LastModifiedAt, Guid CreatedBy, DateTime CreatedAt);
