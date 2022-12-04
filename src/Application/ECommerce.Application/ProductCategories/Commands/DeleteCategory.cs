using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Results.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.ProductCategories.Commands;

public class DeleteCategoryCommand : ICommand
{
    public DeleteCategoryCommand(long id)
    {
        Id = id;
    }
    
    public long Id { get; }
}

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    private readonly IApplicationDatabase _database;

    public DeleteCategoryCommandHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _database.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            return new NotFoundError($"Product category with id {category} not found.");

        _database.Categories.Remove(category);
        await _database.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}