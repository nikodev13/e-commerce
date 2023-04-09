using System.Linq.Expressions;
using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Users.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.Constants;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Users.Queries;

public record GetPaginatedUsersQuery(int PageSize, int PageNumber, string? SearchPhrase, string? SortBy, SortDirection? SortDirection) 
    : IQuery<PaginatedList<UserInListReadModel>>;

internal sealed class GetPaginatedUsersQueryHandler : IQueryHandler<GetPaginatedUsersQuery, PaginatedList<UserInListReadModel>>
{
    private readonly IAppDbContext _dbContext;

    public GetPaginatedUsersQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<PaginatedList<UserInListReadModel>> HandleAsync(GetPaginatedUsersQuery query,
        CancellationToken cancellationToken)
    {
        var (pageSize, pageNumber, searchPhrase, sortBy, sortDirection) = query;

        var baseQuery = _dbContext.Users.AsQueryable();
        var totalCountTask = baseQuery.CountAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(searchPhrase))
        {
            baseQuery = baseQuery.Where(x =>
                x.Email.Contains(searchPhrase.ToLower()) || x.Id.ToString().Contains(searchPhrase));
        }

        Expression<Func<User, object>> func = sortBy?.ToLower() switch
        {
            "email" => x => x.Email,
            _ => x => x.RegisteredAt
        };
        
        baseQuery = sortDirection is SortDirection.ASC
            ? baseQuery.OrderBy(func)
            : baseQuery.OrderByDescending(func);

        var totalCount = await totalCountTask;
        var users = await baseQuery.Skip(pageSize * pageNumber).Take(pageSize)
            .Select(x => UserInListReadModel.From(x))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new PaginatedList<UserInListReadModel>(users, pageSize, pageNumber, totalCount);
    }
}