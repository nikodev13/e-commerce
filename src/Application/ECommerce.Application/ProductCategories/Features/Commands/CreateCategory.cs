using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Products;
using ECommerce.Domain.Shared.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.ProductCategories.Features.Commands;

public class CreateCategoryCommand : IRequest<CategoryDto>
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

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IApplicationDatabase _database;
    private readonly ISnowflakeIdService _idService;

    public CreateCategoryCommandHandler(IApplicationDatabase database, ISnowflakeIdService idService)
    {
        _database = database;
        _idService = idService;
    }
    
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // if category with this name already exists, return AlreadyExistsError
        if (await _database.Categories.AnyAsync(x => x.Name == request.CategoryName, cancellationToken))
        {
            throw new AlreadyExistsException($"Category with name '{request.CategoryName}' already exists.");
        }

        var category = new Category(_idService.GenerateId(), request.CategoryName);
        
        await _database.Categories.AddAsync(category, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);
        var result = CategoryDto.FromCategory(category);
        
        return result;
    }
}