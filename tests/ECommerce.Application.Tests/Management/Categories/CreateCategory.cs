using ECommerce.ApplicationCore.Features.Management.Categories.Commands;
using ECommerce.ApplicationCore.Features.Management.Categories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Tests.Management.Categories;

[Collection(TestingCollection.Name)]
public class CreateCategory
{
    private readonly Testing _testing;

    public CreateCategory(Testing testing)
    {
        _testing = testing;
    }

    [Fact]
    public async Task CreateCategory_Successfully_ReturnReadModel()
    {
        // arrange
        var categoryName = "New Games";
        var command = new CreateCategoryCommand
        {
            Name = categoryName
        };

        // act
        var result = await _testing.ExecuteCommandAsync<CreateCategoryCommand, long>(command);

        //assert
        Assert.NotEqual(0, result);
        
        // clean
        await _testing.GetAppDbContext().Categories.Where(x => x.Name == categoryName).ExecuteDeleteAsync();
    }
    
    [Fact]
    public async Task CreateCategory_WithExistingCategoryName_ThrowAlreadyExistsException()
    {
        // arrange
        var categoryName = DummyCategories.Data[0].Name;
        var command = new CreateCategoryCommand
        {
            Name = categoryName
        };

        // act
        async Task Action() => await _testing.ExecuteCommandAsync<CreateCategoryCommand, long>(command);
        

        //assert
        await Assert.ThrowsAsync<CategoryAlreadyExistsException>(Action);
    }
}