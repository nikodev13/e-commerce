using ECommerce.Application.Shared.Results;
using ECommerce.Application.Shared.Results.Errors;
using ECommerce.Domain.Products.Repositories;
using MediatR;

namespace ECommerce.Application.Categories.Commands;

public class DeleteCategoryCommand : IRequest<Result<None>>
{
    public Guid Id { get; }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<None>>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result<None>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);

        if (category is null)
            return new NotFoundError($"Product category with id {category} not found.");

        await _categoryRepository.DeleteAsync(category);
        
        return None.Value;
    }
}