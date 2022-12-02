using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Results.Errors;
using ECommerce.Domain.Products;
using ECommerce.Domain.Shared.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.ProductCategories.Commands;

public class CreateCategoryCommand : IRequest<Result<CategoryDto>>
{
    public CreateCategoryCommand(string categoryName)
    {
        CategoryName = categoryName;
    }
    
    public string CategoryName { get; }
}

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryName).MinimumLength(3);
    }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
{
    private readonly IApplicationDatabase _database;
    private readonly ISnowflakeIdService _idService;
    private readonly ILogger<CreateCategoryCommand> _logger;

    public CreateCategoryCommandHandler(IApplicationDatabase database, ISnowflakeIdService idService, ILogger<CreateCategoryCommand> logger)
    {
        _database = database;
        _idService = idService;
        _logger = logger;
    }
    
    public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // if category with this name already exists, return AlreadyExistsError
        if (await _database.Categories.AnyAsync(x => x.Name == request.CategoryName, cancellationToken))
        {
            return new AlreadyExistsError($"Category with name '{request.CategoryName}' already exists.");
        }

        var category = new Category(_idService.GenerateId(), request.CategoryName);
        
        await _database.Categories.AddAsync(category, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Created new product category with id {@categoryId}", category.Id.Value);
        var result = CategoryDto.FromCategory(category);
        
        return result;
    }
}