using ECommerce.ApplicationCore.Features.Users.Exceptions;
using ECommerce.ApplicationCore.Features.Users.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Users.Queries;

public record GetUserRoleQuery(string Email) : IQuery<RoleReadModel>;

internal sealed class GetUserRoleQueryHandler : IQueryHandler<GetUserRoleQuery, RoleReadModel>
{
    private readonly IAppDbContext _dbContext;

    public GetUserRoleQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<RoleReadModel> HandleAsync(GetUserRoleQuery query, CancellationToken cancellationToken)
    {
        var userRoleReadModel = await _dbContext.Users
            .Where(x => x.Email == query.Email)
            .Select(x => new RoleReadModel(x.Role.ToString()))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return userRoleReadModel ?? throw new UserNotFoundException(query.Email);
    }
}
