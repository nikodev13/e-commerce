using ECommerce.ApplicationCore.Features.Users.Commands;
using ECommerce.ApplicationCore.Features.Users.Queries;
using ECommerce.ApplicationCore.Shared.Constants;

namespace ECommerce.API.Endpoints.RequestBodies;

public record GetPaginatedUsersRequestParameters(int PageSize, int PageNumber, string? SearchPhrase, string? SortBy, SortDirection? SortDirection)
    : GetPaginatedUsersQuery(PageSize, PageNumber, SearchPhrase, SortBy, SortDirection)
{
    public GetPaginatedUsersQuery ToQuery() => this;
}

public record LoginUserRequestBody(string Email, string Password)
{
    public LoginUserCommand ToCommand() => new(Email, Password);
}

public record RegisterUserRequestBody(string Email, string Password)
{
    public RegisterUserCommand ToCommand() => new(Email, Password);
}

public record RefreshTokenRequestBody(string Email, string RefreshToken)
{
    public RefreshTokenCommand ToCommand() => new(Email, RefreshToken);
}

public record ChangeUserEmailRequestBody(string NewEmail)
{
    public ChangeUserEmailCommand ToCommand() => new(NewEmail);
}