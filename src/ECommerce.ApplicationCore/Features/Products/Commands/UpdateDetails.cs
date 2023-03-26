using ECommerce.ApplicationCore.Features.Categories.Exceptions;
using ECommerce.ApplicationCore.Features.Products.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.ApplicationCore.Features.Products.Commands;

public record UpdateProductDetailsCommand(long Id, string Name, string Description, long CategoryId) : ICommand;

public class UpdateProductDetailsCommandHandler : ICommandHandler<UpdateProductDetailsCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;
    private readonly ILogger<UpdateProductDetailsCommand> _logger;

    public UpdateProductDetailsCommandHandler(IAppDbContext dbContext,
        IUserContextProvider userContextProvider,
        ILogger<UpdateProductDetailsCommand> logger)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
        _logger = logger;
    }
    
    public async ValueTask HandleAsync(UpdateProductDetailsCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product is null)
            throw new ProductNotFoundException(request.Id);

        if (await _dbContext.Categories.AnyAsync(x => x.Id == request.CategoryId, cancellationToken))
            throw new CategoryNotFoundException(request.CategoryId);

        var userId = _userContextProvider.UserId!.Value;
        
        product.Name = request.Name;
        product.Description = request.Description;
        product.CategoryId = request.CategoryId;
        product.LastModifiedBy = userId;
        product.LastModifiedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User with id `{@userId}` updated product details for product with id `{@productId}`.", 
            userId, product.Id);
    }
}

public class UpdateProductDetailsCommandValidator : AbstractValidator<UpdateProductDetailsCommand>
{
    public UpdateProductDetailsCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3);
        RuleFor(x => x.CategoryId)
            .NotEmpty();
    }
}