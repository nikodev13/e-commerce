using AutoMapper;
using ECommerce.Application.Categories;
using ECommerce.Application.Categories.DomainServices;
using ECommerce.Domain.Products;
using ECommerce.Domain.Products.Exceptions;
using ECommerce.Domain.Products.Repositories;
using Moq;

namespace ECommerce.Application.Tests.Categories;

public class DomainServices
{
    private readonly IMock<ICategoryRepository> _mockRepository;

    public DomainServices()
    {
        _mockRepository = CategoryRepositoryMock.GetCategoryRepository();
    }

    [Fact]
    public async Task CategoryUniquenessService_ThrowsCategoryAlreadyExistsException_WhenCreatingTwoSameCategories()
    {
        var repo = _mockRepository.Object;
        var service = new CategoryUniquenessChecker(repo);

        var categoryName = "Kitchen";
        var category1 = await Category.Create(categoryName, service);
        await repo.AddAsync(category1);

        async Task<Category> ShouldThrows() => await Category.Create(categoryName, service);

        await Assert.ThrowsAsync<CategoryAlreadyExistsException>(ShouldThrows);
    }
    
}