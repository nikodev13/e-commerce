using AutoMapper;
using ECommerce.Application.Categories;
using ECommerce.Application.Categories.Commands;
using ECommerce.Application.Categories.DomainServices;
using ECommerce.Application.Categories.Queries;
using ECommerce.Domain.Products.Repositories;
using Moq;

namespace ECommerce.Application.Tests.Categories;

public class CategoryCommands
{
    private readonly IMock<ICategoryRepository> _mockRepository;
    private readonly IMapper _mapper;

    public CategoryCommands()
    {
        _mockRepository = CategoryRepositoryMock.GetCategoryRepository();
        
        var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CategoryMappingProfile>();
            }
        );

        _mapper = configurationProvider.CreateMapper();
    }
    
    [Fact]
    public async Task GetAllCategories_ReturnsAllCategories_NotEmpty()
    {
        var handler = new GetAllCategoriesQueryHandler(_mockRepository.Object, _mapper);

        var result = await handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);
        
        Assert.NotEmpty(result);
    }

    [Theory]
    [InlineData("Coffee")]
    [InlineData("Tea")]
    [InlineData("Sea")]
    public async Task CreateCategory_ShouldBePerfect(string name)
    {
        var mockRepositoryObject = _mockRepository.Object;
        var handler = new CreateCategoryCommandHandler(mockRepositoryObject, new CategoryUniquenessChecker(_mockRepository.Object));

        var result = await handler.Handle(new CreateCategoryCommand(name), CancellationToken.None);

        Assert.True(result.IsSuccess && mockRepositoryObject.GetByNameAsync(name).Result is not null);
    }
}