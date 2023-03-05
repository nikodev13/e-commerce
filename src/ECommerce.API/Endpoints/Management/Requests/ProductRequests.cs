using ECommerce.ApplicationCore.Shared.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints.Management.Requests;

public class GetPaginatedProductsRequest
{
    public int PageSize { get; init; } = 25;
    public int PageNumber { get; init; } = 1;
    public string? SearchPhrase { get; init; }
    public string? SortBy { get; init; }
    public SortDirection? SortDirection { get; init; }
}

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