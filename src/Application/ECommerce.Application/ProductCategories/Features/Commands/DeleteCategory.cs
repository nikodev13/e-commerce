using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.ProductCategories.Features.Commands;

public class DeleteCategoryCommand : IRequest
{
    public DeleteCategoryCommand(long id)
    {
        Id = id;
    }
    
    public long Id { get; }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IApplicationDatabase _database;

    public DeleteCategoryCommandHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _database.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            throw new NotFoundException($"Product category with id {category} not found.");

        _database.Categories.Remove(category);
        await _database.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}