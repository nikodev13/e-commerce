using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Results.Errors;
using ECommerce.Application.Users.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Users.Commands;

public class LoginUserCommand : IRequest<Result<string>>
{
    public string Email { get; }
    public string Password { get; }

    public LoginUserCommand(string email, string password)
    {
        Email = email.ToLower();
        Password = password;
    }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<string>>
{
    private readonly IApplicationDatabase _database;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserContextService _userContextService;

    public LoginUserCommandHandler(IApplicationDatabase database,
        IPasswordHasher passwordHasher,
        ITokenProvider tokenProvider,
        IUserContextService userContextService)
    {
        _database = database;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
        _userContextService = userContextService;
    }

    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        if (_userContextService.UserId is not null)
            return new AuthenticationError("You're already logged in.");
            
        var user = await _database.Users.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken: cancellationToken);
        if (user is null)
            return new AuthenticationError("Invalid email or password.");

        if (!_passwordHasher.ValidatePassword(request.Password, user.PasswordHash))
            return new AuthenticationError("Invalid email or password.");
        
        var result =_tokenProvider.GenerateAccessToken(user);
        return result;
    }
}
