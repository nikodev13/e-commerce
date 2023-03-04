namespace ECommerce.API.Endpoints.Customers;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder RegisterOrderEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Customer Orders";
        
        endpoints.MapGet("api/orders", GetById);
        endpoints.MapGet("api/orders", GetAll);
        endpoints.MapPost("api/orders/place", PlaceOrder);
        
        return endpoints;
    }
    
    private static ValueTask<IResult> GetById(
        
    )
    {
        return ValueTask.FromResult(Results.Ok(""));
    }
    
    private static ValueTask<IResult> GetAll(
        
        )
    {
        return ValueTask.FromResult(Results.Ok(""));
    }
    
    private static ValueTask<IResult> PlaceOrder(
        
    )
    {
        return ValueTask.FromResult(Results.Ok(""));
    }

}