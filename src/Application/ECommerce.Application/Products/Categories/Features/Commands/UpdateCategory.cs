using ECommerce.Application.Shared.Interfaces;
using ECommerce.Application.Shared.Results;
using ECommerce.Application.Shared.Results.Errors;
using ECommerce.Domain.Products.Categories.Exceptions;
using ECommerce.Domain.Products.Categories.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Products.Categories.Features.Commands;

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
    private readonly ICategoryUniquenessChecker _uniquenessChecker;

    public UpdateCategoryCommandHandler(IApplicationDatabase database, ICategoryUniquenessChecker uniquenessChecker)
    {
        _database = database;
        _uniquenessChecker = uniquenessChecker;
    }
    
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _database.Categories
            .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (category is null)
            return new NotFoundError($"Product category with id {category} not found.");

        try
        {
            category.ChangeName(request.CategoryName, _uniquenessChecker);
        }
        catch (CategoryAlreadyExistsException exception)
        {
            return new AlreadyExistsError(exception.Message);
        }
        
        await _database.SaveChangesAsync(cancellationToken);
            
        return Result.Success();
    }
}