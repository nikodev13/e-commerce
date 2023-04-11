using ECommerce.ApplicationCore.Features.Users.Exceptions;
using ECommerce.ApplicationCore.Features.Users.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Users.Queries;

public record GetUserRoleQuery(string Email) : IQuery<UserRoleReadModel>;

internal sealed class GetUserRoleQueryHandler : IQueryHandler<GetUserRoleQuery, UserRoleReadModel>
{
    private readonly IAppDbContext _dbContext;

    public GetUserRoleQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<UserRoleReadModel> HandleAsync(GetUserRoleQuery query, CancellationToken cancellationToken)
    {
        var userRoleReadModel = await _dbContext.Users
            .Select(x => new UserRoleReadModel(x.Email, x.Role.ToString()))
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == query.Email, cancellationToken);

        return userRoleReadModel ?? throw new UserNotFoundException(query.Email);
    }
}
