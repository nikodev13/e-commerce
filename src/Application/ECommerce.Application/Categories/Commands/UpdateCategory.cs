using ECommerce.Application.Interfaces;
using ECommerce.Application.Shared.Results;
using ECommerce.Application.Shared.Results.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Categories.Commands;

public class UpdateCategoryCommand : IRequest<Result>
{
    public long CategoryId { get; }
    public string CategoryName { get; }

    public UpdateCategoryCommand(long categoryId, string categoryName)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
    }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
{
    private readonly IApplicationDatabase _database;

    public UpdateCategoryCommandHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (await _database.Categories.AnyAsync(c => c.Name == request.CategoryName, cancellationToken))
        {
            return new AlreadyExistsError($"Category with name {request.CategoryName} already exists.");
        }
        
        var category = await _database.Categories
            .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (category is null)
            return new NotFoundError($"Product category with id {category} not found.");

        category.Name = request.CategoryName;
        await _database.SaveChangesAsync(cancellationToken);
            
        return Result.Success();
    }
}