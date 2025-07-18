using ServiceDesk.Domain.Database;
using ServiceDesk.Infrastructure.Database;
using ServiceDesk.Application.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceDesk.Infrastructure.Repository;

public abstract class Repository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly ServiceDeskDbContext _db;

    public Repository(ServiceDeskDbContext db)
    {
        _db = db;
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

    public async Task<List<T>> GetAll(int limit = 10, int offset = 0, string? sort = null, bool noTracking = false, CancellationToken ct = default)
    {
        var query = GetQuery(sort, noTracking);  
        return await query.Skip(offset).Take(limit).ToListAsync(ct);
    }

    public virtual async Task<T?> GetBy(int id, bool noTracking = false, CancellationToken ct = default)
    {
        var query = GetQuery(noTracking: noTracking);
        return await query.Where(x => x.Id == id).FirstOrDefaultAsync(ct);
    }

    public virtual async Task CreateAsync(T entity, CancellationToken ct)
    {
        DbSet.Add(entity);
        await _db.SaveChangesAsync(ct);
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken ct)
    {
        DbSet.Update(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, T entity, CancellationToken ct)
    {
        DbSet.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }
}
