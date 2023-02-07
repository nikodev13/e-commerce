namespace ECommerce.API.Endpoints.Management.Requests;

public class CreateCategoryRequest
{
    public required string Name { get; init; }
}

public class UpdateCategoryRequest
{
    public required string Name { get; init; }
}
