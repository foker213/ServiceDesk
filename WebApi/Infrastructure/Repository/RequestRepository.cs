using ServiceDesk.Domain.Database.Models;
using ServiceDesk.Infrastructure.Database;
using ServiceDesk.Application.IRepository;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Domain.Database;
using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts;

namespace ServiceDesk.Infrastructure.Repository;

internal sealed class RequestRepository(
    ServiceDeskDbContext db,
    ICurrentUserService userService
) : Repository<Request>(db), IRequestRepository
{
    public async Task<PagingModel<Request>> GetAll(int limit = 10, int offset = 0, string? sort = null, string dictionaryType = "", CancellationToken ct = default)
    {
        int userId = userService.UserId;
        var query = GetQueryFactory(dictionaryType, userId)(sort);
        List<Request> result = await query.Skip(offset).Take(limit).ToListAsync(ct);

        return new(result.Count(), result);
    }

    private Func<string?, IQueryable<Request>> GetQueryFactory(string dictionaryType, int userId)
    {
        return dictionaryType switch
        {
            "archive" => GetArchiveQuery,
            "myRequests" => sort => GetMyRequestsQuery(sort, userId),
            "undistributedRequests" => GetUnclaimedRequestsQuery,
            _ => throw new ArgumentException($"Unknown dictionary type: {dictionaryType}")
        };
    }

    private IQueryable<Request> GetArchiveQuery(string? sort)
    {
        var query = GetQuery(sort);
        return query.Where(x => x.Status == Status.Solved);
    }

    private IQueryable<Request> GetMyRequestsQuery(string? sort, int userId)
    {
        var query = GetQuery(sort);
        return query.Where(x => x.UserId == userId);
    }

    private IQueryable<Request> GetUnclaimedRequestsQuery(string? sort)
    {
        var query = GetQuery(sort);
        return query.Where(x => x.Status == Status.NotAssigned);
    }

    public async Task<List<Request>> GetByExternalUserId(int externalUserId, CancellationToken ct)
    {
        var query = GetQuery();
        return await query.Where(x => x.Chat!.ExternalUserId == externalUserId).ToListAsync(ct);
    }
}
