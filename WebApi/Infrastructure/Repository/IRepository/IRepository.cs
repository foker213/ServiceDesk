using Domain.DataBase;

namespace Application.Repository;

public interface IRepository<T> where T : class, IEntity
{
    Task<List<T>> GetAll(int limit = 10, int offset = 0, string? sort = null, bool noTracking = false);
    Task<T?> GetBy(int id, bool noTracking = false);
    Task Create(T entity);
    Task Update(T entity);
    Task Delete(int id);
    Task<int> Count();
}
