using ECommerce.ApplicationCore.Features.Products.Commands;
using ECommerce.ApplicationCore.Features.Products.Queries;

namespace ECommerce.API.Endpoints.Requests;

public class GetPaginatedCustomerProductsRequest : GetPaginatedCustomerProductsQuery { }

public record CreateProductRequest(string Name, string Description, long CategoryId, decimal Price, uint Quantity, bool IsActive)
{
    public CreateProductCommand ToCommand() => new(Name, Description, CategoryId, Price, Quantity, IsActive);
}

public record UpdateProductDetailsRequest(string Name, string Description, long CategoryId)
{
    public UpdateProductDetailsCommand ToCommand(long productId) => new(productId, Name, Description, CategoryId);
}

public record UpdateProductSaleDataRequest(decimal Price, uint Quantity, bool IsActive)
{
    public UpdateProductSaleDataCommand ToCommand(long productId) => new(productId, Price, Quantity, IsActive);
}