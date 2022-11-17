using ECommerce.Application.Shared.Results;
using ECommerce.Application.Shared.Results.Errors;
using ECommerce.Domain.Products;
using ECommerce.Domain.Products.Exceptions;
using ECommerce.Domain.Products.Repositories;
using ECommerce.Domain.Products.Services;
using MediatR;

namespace ECommerce.Application.Categories.Commands;

public class CreateCategoryCommand : IRequest<Result<Guid>>
{
    public CreateCategoryCommand(string categoryName)
    {
        CategoryName = categoryName;
    }
    
    public string CategoryName { get; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryUniquenessChecker _uniquenessChecker;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, ICategoryUniquenessChecker uniquenessChecker)
    {
        _categoryRepository = categoryRepository;
        _uniquenessChecker = uniquenessChecker;
    }
    
    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {   
        try
        {
            var category = await Category.Create(request.CategoryName, _uniquenessChecker);
            await _categoryRepository.AddAsync(category);
            return category.Id.Value;
        }
        catch (CategoryAlreadyExistsException exception)
        {
            return new AlreadyExistsError(exception.Message);
        }
    }
}