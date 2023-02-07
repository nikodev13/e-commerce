namespace ECommerce.API.Endpoints.Management.Requests;

public class CreateProductRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required long CategoryId { get; init; }
    public required decimal Price { get; init; }
    public required int Quantity { get; init; }
    public required bool IsActive { get; init; }
}

public class UpdateProductDetailsRequest
{
    public required long Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required long CategoryId { get; init; }
}

public class UpdateProductSaleDataRequest
{
    public required long Id { get; init; }
    public required decimal Price { get; init; }
    public required int Quantity { get; init; }
    public required bool IsActive { get; init; }
}