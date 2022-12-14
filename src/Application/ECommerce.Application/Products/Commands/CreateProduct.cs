using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Results.Errors;
using ECommerce.Application.Products.Models;
using ECommerce.Domain.Products;
using ECommerce.Domain.Shared.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Products.Commands;

public class CreateProductCommand : ICommand<ProductDto>
{
    public string Name { get; }
    public string Description { get; }
    public long CategoryId { get;  }

    public CreateProductCommand(string name, string description, long categoryId)
    {
        Name = name;
        Description = description;
        CategoryId = categoryId;
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
    }
}

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductDto>
{
    private readonly IApplicationDatabase _database;
    private readonly ISnowflakeIdService _idService;

    public CreateProductCommandHandler(IApplicationDatabase database, ISnowflakeIdService idService)
    {
        _database = database;
        _idService = idService;
    }
    
    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categoryExists = await _database.Categories
            .AnyAsync(x => x.Id == request.CategoryId, cancellationToken);

        if (!categoryExists)
            return new BadRequestError($"Category with id {request.CategoryId} does not exist.");
            
        var category = new Product
        {
            Name = request.Name,
            Description = request.Description
            
        };

        var result = ProductDto.FromProduct(category);
        return result;
    }
}