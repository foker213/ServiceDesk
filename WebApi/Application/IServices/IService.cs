using ServiceDesk.Application.IRepository;
using ServiceDesk.Contracts;
using ServiceDesk.Domain.Database;

namespace ServiceDesk.Application.IServices;

public interface IService<TRequest, TResponse, TRepository, TEntity>
    where TRequest : class
    where TResponse : class
    where TRepository : IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<PagingModel<TResponse>> GetAll(int? pageSize, int? pageIndex, string? sort);
    Task<OperationResult<TResponse>> GetBy(int id);
    Task<OperationResult<TResponse>> CreateAsync(TRequest request);
    Task<OperationResult<bool>> UpdateAsync(int id, TRequest request);
    Task<OperationResult<bool>> DeleteAsync(int id);
}