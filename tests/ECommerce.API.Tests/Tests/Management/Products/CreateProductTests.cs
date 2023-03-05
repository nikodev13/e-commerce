using System.Net;
using System.Text;
using ECommerce.API.Endpoints.Management.Requests;
using ECommerce.API.Tests.Configuration;
using ECommerce.API.Tests.DummyData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ECommerce.API.Tests.Tests.Management.Products;

[Collection(TestingCollection.Name)]
public class CreateProductTest
{
    private readonly Testing _testing;

    public CreateProductTest(Testing testing)
    {
        _testing = testing;
    }

    [Fact]
    public async Task Create_Successfully_Return201Created()
    {
        // arrange
        var db = _testing.GetDbContext();
        FakeUserContextProvider.CurrentUserId = DummyUsers.Data[0].Id;
        var client = _testing.Client;
        
        var request = new CreateProductRequest
        {
            Name = "New Product",
            Description = "",
            CategoryId = DummyCategories.Data[1].Id,
            Price = 12,
            Quantity = 10,
            IsActive = true
        };
        var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // act
        var response = await client.PostAsync("api/management/products", httpContent);
        var location = response.Headers.Location!.OriginalString;
        long.TryParse(location.AsSpan(location.LastIndexOf('/') + 1), out var id);
        var product = await db.Products.FirstOrDefaultAsync(x => x.Id == id);
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        product.Should().NotBeNull();
        product!.Name.Should().Be(request.Name);
        
        // clean
        await db.Products.Where(x => x.Name == request.Name).ExecuteDeleteAsync();
    }
}