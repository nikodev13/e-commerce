using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Results.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.ProductCategories.Commands;

public class DeleteCategoryCommand : IRequest<Result>
{
    public DeleteCategoryCommand(long id)
    {
        Id = id;
    }
    
    public long Id { get; }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
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