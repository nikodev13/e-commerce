using ECommerce.ApplicationCore.Features.Wishlist.Commands;

namespace ECommerce.API.Endpoints.Requests;

public record AddProductToWishlistRequest(long ProductId) : AddProductToWishlistCommand(ProductId)
{
    public AddProductToWishlistCommand ToCommand() => this;
}

public record RemoveProductFromWishlistRequest(long ProductId) : RemoveProductFromWishlistCommand(ProductId)
{
    public RemoveProductFromWishlistCommand ToCommand() => this;
}