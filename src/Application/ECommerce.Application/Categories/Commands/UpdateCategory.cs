using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Categories.Commands;

public class UpdateCategoryCommand : ICommand 
{
    public long CategoryId { get; }
    public string CategoryName { get; }

    public UpdateCategoryCommand(long categoryId, string categoryName)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
    }
}

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty();
        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .MinimumLength(3);
    }
}

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly IApplicationDatabase _database;

    public UpdateCategoryCommandHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        // if category with this name already exists, return AlreadyExistsError
        if (await _database.Categories.AnyAsync(x => x.Name == request.CategoryName, cancellationToken))
        {
            throw new AlreadyExistsException($"Category with name '{request.CategoryName}' already exists.");
        }
        
        var category = await _database.Categories
            .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (category is null)
            throw new NotFoundException($"Product category with id {category} not found.");

        category.Name = request.CategoryName;
        await _database.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}