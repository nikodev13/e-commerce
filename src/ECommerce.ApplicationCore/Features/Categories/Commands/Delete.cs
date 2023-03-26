using ECommerce.ApplicationCore.Features.Categories.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.ApplicationCore.Features.Categories.Commands;

public record DeleteCategoryCommand(long Id) : ICommand;

public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;
    private readonly ILogger<DeleteCategoryCommandHandler> _logger;

    public DeleteCategoryCommandHandler(IAppDbContext dbContext,
        IUserContextProvider userContextProvider,
        ILogger<DeleteCategoryCommandHandler> logger)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
        _logger = logger;
    }
    
    public async ValueTask HandleAsync(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (category is null)
            throw new CategoryNotFoundException(request.Id);

        if (await _dbContext.Products.AnyAsync(x => x.Category.Id == request.Id, cancellationToken))
            throw new CategoryWithProductsCannotBeDeletedException();

        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("User with id `{@userId}` deleted product category (id `{@categoryId}`) with name {@categoryName}.",
            _userContextProvider.UserId, category.Id, category.Name);
    }
}