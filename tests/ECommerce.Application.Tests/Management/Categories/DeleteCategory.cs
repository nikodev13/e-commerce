using ECommerce.ApplicationCore.Features.Management.Categories.Commands;
using ECommerce.ApplicationCore.Features.Management.Categories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Tests.Management.Categories;

[Collection(TestingCollection.Name)]
public class DeleteCategory
{
    private readonly Testing _testing;

    public DeleteCategory(Testing testing)
    {
        _testing = testing;
    }

    [Fact]
    public async Task DeleteCategory_Successfully()
    {
        // arrange
        var db = _testing.GetAppDbContext();
        var id = DummyCategories.Data[2].Id;
        var command = new DeleteCategoryCommand
        {
            Id = id
        };

        // act
        await _testing.ExecuteCommandAsync(command);

        // assert
        var exist = await db.Categories.AnyAsync(x => x.Id == id);
        Assert.False(exist);
        
        // clean
        await db.Categories.AddAsync(DummyCategories.Data[2]);
        await db.SaveChangesAsync(default);
    }
    
    [Fact]
    public async Task DeleteCategory_WithProducts_ThrowsCategoryWithProductsCannotBeDeletedException()
    {
        // arrange
        var id = DummyCategories.Data[0].Id;
        var command = new DeleteCategoryCommand
        {
            Id = id
        };

        // act
        async Task Action() => await _testing.ExecuteCommandAsync(command);

        // assert
        await Assert.ThrowsAsync<CategoryWithProductsCannotBeDeletedException>(Action);
    }
}