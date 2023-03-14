using ECommerce.ApplicationCore.Features.Management.Products.Queries;
using ECommerce.ApplicationCore.Shared.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints.Management.Requests;

public class GetPaginatedManagementProductsRequest : GetPaginatedManagementProductsQuery { }

public class CreateProductRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required long CategoryId { get; init; }
    public required decimal Price { get; init; }
    public required uint Quantity { get; init; }
    public required bool IsActive { get; init; }
}

public class UpdateProductDetailsRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required long CategoryId { get; init; }
}

public class UpdateProductSaleDataRequest
{
    public required decimal Price { get; init; }
    public required uint Quantity { get; init; }
    public required bool IsActive { get; init; }
}