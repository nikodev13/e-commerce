using ECommerce.Application.Management.Categories.Exceptions;
using ECommerce.Application.Management.Products.Exceptions;
using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Shared.CQRS;
using ECommerce.Domain.Management.Entities;
using ECommerce.Domain.Shared.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Management.Products.Commands;

public class CreateProductCommand : ICommand<ProductReadModel>
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required long CategoryId { get; init; }
    public required decimal Price { get; init; }
    public required int Quantity { get; init; }
    public required bool IsActive { get; init; }
}

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductReadModel>
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
    
    public async Task<ProductReadModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.Products.AnyAsync(x => x.Name == request.Name, cancellationToken))
            throw new ProductAlreadyExistsException(request.Name);

        var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);
        if (category is null)
            throw new CategoryNotFoundException(request.CategoryId);
            
        var product = Product.CreateNew(request.Name, request.Description, category, request.Price, request.Quantity, request.IsActive, _idProvider);

        await _dbContext.Products.AddAsync(product, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User with id `{@userId}` created product with id `{@productId}`.",
            _userContextProvider.UserId, product.Id.Value);
        
        var result = ProductReadModel.FromProduct(product);
        return result;
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