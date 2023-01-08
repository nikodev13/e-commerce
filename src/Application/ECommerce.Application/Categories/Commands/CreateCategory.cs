using ECommerce.Application.Categories.ReadModels;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Products;
using ECommerce.Domain.Shared.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Categories.Commands;

public class CreateCategoryCommand : ICommand<CategoryReadModel>
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
        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .MinimumLength(3);
    }
}

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryReadModel>
{
    private readonly IApplicationDatabase _database;
    private readonly ISnowflakeIdService _idService;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    public CreateCategoryCommandHandler(IApplicationDatabase database, ISnowflakeIdService idService, ILogger<CreateCategoryCommandHandler> logger)
    {
        _database = database;
        _idService = idService;
        _logger = logger;
    }
    
    public async Task<CategoryReadModel> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // if category with this name already exists, return AlreadyExistsError
        if (await _database.Categories.AnyAsync(x => x.Name == request.CategoryName, cancellationToken))
        {
            throw new AlreadyExistsException($"Category with name '{request.CategoryName}' already exists.");
        }

        var category = Category.CreateNew(request.CategoryName, _idService);
        
        await _database.Categories.AddAsync(category, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Created new product category with id {@categoryId}", category.Id.Value);
        var result = CategoryReadModel.FromCategory(category);
        
        return result;
    }
}