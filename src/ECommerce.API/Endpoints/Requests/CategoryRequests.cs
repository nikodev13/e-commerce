using ECommerce.ApplicationCore.Features.Categories.Commands;

namespace ECommerce.API.Endpoints.Requests;

public record CreateCategoryRequest(string Name)
{
    public CreateCategoryCommand ToCommand() => new(Name);
}

public record UpdateCategoryRequest(string Name)
{
    public UpdateCategoryCommand ToCommand(long categoryId) => new(categoryId, Name);
}
