using ECommerce.ApplicationCore.Features.Wishlist.Commands;

namespace ECommerce.API.Endpoints.RequestBodies;

public record AddProductToWishlistRequestBody(long ProductId) : AddProductToWishlistCommand(ProductId)
{
    public AddProductToWishlistCommand ToCommand() => this;
}

public record RemoveProductFromWishlistRequestBody(long ProductId) : RemoveProductFromWishlistCommand(ProductId)
{
    public RemoveProductFromWishlistCommand ToCommand() => this;
}