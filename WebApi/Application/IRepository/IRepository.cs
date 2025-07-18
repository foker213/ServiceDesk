using ServiceDesk.Domain.Database;

namespace ServiceDesk.Application.IRepository;

public interface IRepository<T> where T : class, IEntity
{
    Task<List<T>> GetAll(int limit = 10, int offset = 0, string? sort = null, bool noTracking = false, CancellationToken ct = default);
    Task<T?> GetBy(int id, bool noTracking = false, CancellationToken ct = default);
    Task CreateAsync(T entity, CancellationToken ct);
    Task UpdateAsync(T entity, CancellationToken ct);
    Task DeleteAsync(int id, T entity, CancellationToken ct);
}
