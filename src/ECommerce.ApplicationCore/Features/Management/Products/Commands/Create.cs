using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Management.Categories.Exceptions;
using ECommerce.ApplicationCore.Features.Management.Products.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.ApplicationCore.Features.Management.Products.Commands;

public class CreateProductCommand : ICommand<long>
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required long CategoryId { get; init; }
    public required decimal Price { get; init; }
    public required uint Quantity { get; init; }
    public required bool IsActive { get; init; }
}

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, long>
{
    private readonly IAppDbContext _dbContext;
    private readonly ISnowflakeIdProvider _idProvider;
    private readonly IUserContextProvider _userContextProvider;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(IAppDbContext dbContext, 
        ISnowflakeIdProvider idProvider,
        IUserContextProvider userContextProvider,
        ILogger<CreateProductCommandHandler> logger)
    {
        _dbContext = dbContext;
        _idProvider = idProvider;
        _userContextProvider = userContextProvider;
        _logger = logger;
    }
    
    public async ValueTask<long> HandleAsync(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContextProvider.UserId!.Value;
        
        if (await _dbContext.Products.AnyAsync(x => x.Name == request.Name, cancellationToken))
            throw new ProductAlreadyExistsException(request.Name);

        var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);
        if (category is null)
            throw new CategoryNotFoundException(request.CategoryId);
            
        var product = new Product
        {
            Id = _idProvider.GenerateId(),
            Name = request.Name,
            Description = request.Description,
            Category = category,
            Price = request.Price,
            InStockQuantity = request.Quantity,
            IsActive = request.IsActive,
            CreatedBy = userId,
            CreatedAt = DateTime.Now
        };

        await _dbContext.Products.AddAsync(product, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User with id `{@userId}` created product with id `{@productId}`.",
            userId, product.Id);

        return product.Id;
    }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3);
        RuleFor(x => x.CategoryId)
            .NotEmpty();
        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0);
        RuleFor(x => x.Quantity)
            .NotEmpty();
    }
}