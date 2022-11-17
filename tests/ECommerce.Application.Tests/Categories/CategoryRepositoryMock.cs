using ECommerce.Application.Categories;
using ECommerce.Domain.Products;
using ECommerce.Domain.Products.Repositories;
using ECommerce.Domain.Products.Services;
using ECommerce.Domain.Products.ValueObjects;
using Moq;

namespace ECommerce.Application.Tests.Categories;

public static class CategoryRepositoryMock
{
    public static IMock<ICategoryRepository> GetCategoryRepository()
    {
        var categories = GetCategories().Result;
        var mockRepository = new Mock<ICategoryRepository>();

        mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(categories);
        mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<CategoryId>()))
            .ReturnsAsync((Guid id) => categories.FirstOrDefault(x => x.Id == id));
        mockRepository.Setup(x => x.GetByNameAsync(It.IsAny<CategoryName>()))
            .ReturnsAsync((CategoryName name) => categories.FirstOrDefault(x => x.Name.Value == name.Value));
        mockRepository.Setup(x => x.AddAsync(It.IsAny<Category>()))
            .Callback<Category>(category => categories.Add(category));
        mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Category>()))
            .Callback<Category>(category =>
            {
                var toRemove = categories.First(x => x.Id.Value == category.Id.Value);
                categories.Remove(toRemove);
                categories.Add(category);
            });
        mockRepository.Setup(x => x.DeleteAsync(It.IsAny<Category>()))
            .Callback<Category>(category =>
            {
                var toRemove = categories.First(x => x.Id.Value == category.Id.Value);
                categories.Remove(toRemove);
            });
        
        return mockRepository;
    }

    public async static Task<List<Category>> GetCategories()
    {
        var fakeUniquenessChecker = new FakeCategoryUniquenessChecker(true);
        return new List<Category>()
        {
            await Category.Create("AA", fakeUniquenessChecker),
            await Category.Create("BB", fakeUniquenessChecker),
            await Category.Create("CC", fakeUniquenessChecker),
            await Category.Create("DD", fakeUniquenessChecker),
        };
    }
    
    public class FakeCategoryUniquenessChecker : ICategoryUniquenessChecker
    {
        private readonly bool _isAlwaysUnique;

        public FakeCategoryUniquenessChecker(bool isAlwaysUnique)
        {
            _isAlwaysUnique = isAlwaysUnique;
        }
        
        public Task<bool> IsUnique(CategoryName name)
        {
            return Task.FromResult(_isAlwaysUnique);
        }
    }
}