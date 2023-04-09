using ECommerce.ApplicationCore.Features.Categories.Commands;

namespace ECommerce.API.Endpoints.RequestBodies;

public record CreateCategoryRequestBody(string Name)
{
    public CreateCategoryCommand ToCommand() => new(Name);
}

public record UpdateCategoryRequestBody(string Name)
{
    public UpdateCategoryCommand ToCommand(long categoryId) => new(categoryId, Name);
}
