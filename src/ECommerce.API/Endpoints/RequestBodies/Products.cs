using ECommerce.ApplicationCore.Features.Products.Commands;
using ECommerce.ApplicationCore.Features.Products.Queries;

namespace ECommerce.API.Endpoints.RequestBodies;

public class GetPaginatedCustomerProductsRequestParameters : GetPaginatedCustomerProductsQuery { }

public record CreateProductRequestBody(string Name, string Description, long CategoryId, decimal Price, uint Quantity, bool IsActive)
{
    public CreateProductCommand ToCommand() => new(Name, Description, CategoryId, Price, Quantity, IsActive);
}

public record UpdateProductDetailsRequestBody(string Name, string Description, long CategoryId)
{
    public UpdateProductDetailsCommand ToCommand(long productId) => new(productId, Name, Description, CategoryId);
}

public record UpdateProductSaleDataRequestBody(decimal Price, uint Quantity, bool IsActive)
{
    public UpdateProductSaleDataCommand ToCommand(long productId) => new(productId, Price, Quantity, IsActive);
}