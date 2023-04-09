using ECommerce.Application.Tests.Products;
using ECommerce.Application.Tests.Users;
using ECommerce.Application.Tests.Utilities;
using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Orders.Commands;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Tests.Customers.Orders;

[Collection(TestingCollection.Name)]
public class PlaceOrder
{
    private readonly Testing _testing;

    public PlaceOrder(Testing testing)
    {
        _testing = testing;
    }

    [Fact]
    public async Task PlacingOrderSuccessfully()
    {
        // arrange
        FakeUserContextProvider.CurrentUserId = DummyUsers.Data[0].Id;
        var db = _testing.GetAppDbContext();
        var product = DummyProducts.Data[0];
        var command = new PlaceOrderCommand(new List<PlaceOrderCommand.OrderLine>
            {
                new(product.Id, 1)
            },
            new PlaceOrderCommand.DeliveryOptions(DeliveryOperator.InPost, "any-street", "00-000", "Danzig"));
        // act
        var lastProductQuantity = DummyProducts.Data[0].InStockQuantity;
        var result = await _testing.ExecuteCommandAsync<PlaceOrderCommand, long>(command);
        // assert
        var order = await db.Orders.FirstOrDefaultAsync(x => x.Id == result);
        var dbProduct = await db.Products.FirstOrDefaultAsync(x => x.Id == product.Id);
        
        Assert.NotNull(order);
        Assert.Equal(dbProduct!.InStockQuantity, lastProductQuantity-1);
    }
}