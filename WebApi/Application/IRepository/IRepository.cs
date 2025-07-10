using ServiceDesk.Domain.Database;

namespace ServiceDesk.Application.IRepository;

public interface IRepository<T> where T : class, IEntity
{
    Task<List<T>> GetAll(int limit = 10, int offset = 0, string? sort = null, bool noTracking = false);
    Task<T?> GetBy(int id, bool noTracking = false);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<int> Count();
}
