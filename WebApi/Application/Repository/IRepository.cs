using Domain.DataBase;

namespace Application.Repository;

public interface IRepository<T> where T : class, IEntity
{
    Task<T?> GetById(int id, bool noTracking = false);
    Task Create(T entity);
    Task Update(T entity);
    Task Delete(int id);

}
