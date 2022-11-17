using ECommerce.Application.Shared;
using ECommerce.Application.Shared.Results;
using ECommerce.Application.Shared.Results.Errors;
using ECommerce.Domain.Products.Repositories;
using ECommerce.Domain.Products.ValueObjects;
using MediatR;

namespace ECommerce.Application.Categories.Commands;

public class UpdateCategoryCommand : IRequest<Result<None>>
{
    public Guid CategoryId { get; }
    public string CategoryName { get; }

    public UpdateCategoryCommand(Guid categoryId, string categoryName)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
    }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<None>>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result<None>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

        if (category is null)
            return new NotFoundError($"Product category with id {category} not found.");

        category.Name = request.CategoryName;
        await _categoryRepository.UpdateAsync(category);
            
        return None.Value;
    }
}