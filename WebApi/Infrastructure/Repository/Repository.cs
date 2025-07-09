using ServiceDesk.Domain.Database;
using ServiceDesk.Infrastructure.Database;
using ServiceDesk.Application.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceDesk.Infrastructure.Repository;

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

        var fieldName = sort.Trim('-').ToLower();
        var parameter = Expression.Parameter(typeof(T));
        MemberExpression property;

        if (fieldName.Contains('.'))
        {
            // find nested property
            var propertyPath = fieldName.Split('.');
            property = Expression.Property(parameter, propertyPath[0]);
            foreach (var member in propertyPath.Skip(1))
                property = Expression.Property(property, member);
        }
        else
        {
            property = Expression.Property(parameter, fieldName);
        }

        var propAsObject = Expression.Convert(property, typeof(object));
        var expression = Expression.Lambda<Func<T, object>>(propAsObject, parameter);

        if (sort.StartsWith('-'))
            query = query.OrderByDescending(expression);
        else
            query = query.OrderBy(expression);

        return query;
    }

    public async Task<int> Count()
    {
        var query = GetQuery(noTracking: true);
        return await query.CountAsync();
    }

    public async Task<List<T>> GetAll(int limit = 10, int offset = 0, string? sort = null, bool noTracking = false)
    {
        var query = GetQuery(sort, noTracking);  
        return await query.Skip(offset).Take(limit).ToListAsync();
    }

    public async Task<T?> GetBy(int id, bool noTracking = false)
    {
        var query = GetQuery(noTracking: noTracking);
        return await query.Where(x => x.Id == id).FirstOrDefaultAsync();
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
        DbSet.Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await GetBy(id) ?? throw new ArgumentNullException(nameof(id));
        DbSet.Remove(entity);
        await _db.SaveChangesAsync();
    }
}
