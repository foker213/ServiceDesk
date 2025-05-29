using Application.Repository;
using Domain.DataBase;
using Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public abstract class Repository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly ServiceDeskDbContext _db;
    protected readonly TimeProvider _timeProvider;

    public Repository(ServiceDeskDbContext db, TimeProvider timeProvider)
    {
        _db = db;
        _timeProvider = timeProvider;
    }

    protected DbSet<T> DbSet => _db.Set<T>();

    protected virtual IQueryable<T> GetQuery(string? sort = null, bool noTracking = false)
    {
        var query = noTracking
            ? DbSet.AsNoTracking()
            : DbSet.AsQueryable();

        if (sort == null || sort == "-id")
        {
            query = query.OrderByDescending(f => f.Id);
            return query;
        }
        else if (sort == "id")
        {
            query = query.OrderBy(f => f.Id);
            return query;
        }

        return query;
    }


    public async Task<T?> GetById(int id, bool noTracking = false)
    {
        var query = GetQuery(noTracking: noTracking);
        return await query
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public virtual async Task Create(T entity)
    {
        entity.CreatedAt = _timeProvider.GetUtcNow().UtcDateTime;
        DbSet.Add(entity);
        await _db.SaveChangesAsync();
    }

    public virtual async Task Update(T entity)
    {
        entity.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;

        // оптимизировать, чтобы обновлять только одно поле

        DbSet.Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await GetById(id); // обработать если ошибка
        DbSet.Remove(entity);
        await _db.SaveChangesAsync();
    }
}
