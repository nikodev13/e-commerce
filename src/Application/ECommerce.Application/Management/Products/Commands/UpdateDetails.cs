using ECommerce.Application.Management.Categories.Exceptions;
using ECommerce.Application.Management.Products.Exceptions;
using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Shared.CQRS;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Management.Products.Commands;

public class UpdateProductDetailsCommand : ICommand
{
    public required long Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required long CategoryId { get; init; }
}

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
    
    public async Task<Unit> Handle(UpdateProductDetailsCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product is null)
            throw new ProductNotFoundException(request.Id);

        var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);
        if (category is null)
            throw new CategoryNotFoundException(request.CategoryId);
        
        product.Name = request.Name;
        product.Description = request.Description;
        product.Category = category;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User with id `{@userId}` updated product details for product with id `{@productId}`.",
            _userContextProvider.UserId, product.Id.Value);
        
        return Unit.Value;
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