using Domain.DataBase.Enums;
using Domain.DataBase.Models;
using Infrastructure.DataBase;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

internal sealed class RequestRepository(
    ServiceDeskDbContext db,
    TimeProvider tp,
    ICurrentUserService userService
) : Repository<Request>(db, tp), IRequestRepository
{
    public async Task<List<Request>> GetAll(int limit = 10, int offset = 0, string? sort, string dictionaryType)
    {
        var query = GetQueryFactory(dictionaryType)(sort);
        return await query.Skip(offset).Take(limit).ToListAsync();
    }

    public Task Update(int id, Request request)
    {

    }

    private Func<string?, IQueryable<Request>> GetQueryFactory(string dictionaryType)
    {
        return dictionaryType switch
        {
            "archive" => GetArchiveQuery,
            "myRequests" => GetMyRequestsQuery,
            "unclaimedRequests" => GetUnclaimedRequestsQuery,
            _ => throw new ArgumentException($"Unknown dictionary type: {dictionaryType}")
        };
    }

    private IQueryable<Request> GetArchiveQuery(string? sort)
    {
        var query = GetQuery(sort);
        return query.Where(x => x.Status == Status.Solved);
    }

    private IQueryable<Request> GetMyRequestsQuery(string? sort, )
    {
        var query = GetQuery(sort);
        return query.Where(x => x.UserId == );
    }

    private IQueryable<Request> GetUnclaimedRequestsQuery(string? sort)
    {
        var query = GetQuery(sort);
        return query.Where(x => x.Status == Status.NotAssigned);
    }
}
