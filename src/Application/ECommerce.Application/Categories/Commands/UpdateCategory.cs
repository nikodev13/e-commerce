using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Categories.Commands;

public class UpdateCategoryCommand : ICommand 
{
    public long Id { get; }
    public string CategoryName { get; }

    public UpdateCategoryCommand(long id, string categoryName)
    {
        Id = id;
        CategoryName = categoryName;
    }
}

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .MinimumLength(3);
    }
}

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly IApplicationDatabase _database;
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;

    public UpdateCategoryCommandHandler(IApplicationDatabase database, ILogger<UpdateCategoryCommandHandler> logger)
    {
        _database = database;
        _logger = logger;
    }
    
    public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        // if category with this name already exists, return AlreadyExistsError
        if (await _database.Categories.AnyAsync(x => x.Name == request.CategoryName, cancellationToken))
        {
            throw new AlreadyExistsException($"Category with name '{request.CategoryName}' already exists.");
        }
        
        var category = await _database.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            throw new NotFoundException($"Product category with id {category} not found.");

        category.Name = request.CategoryName;
        await _database.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}