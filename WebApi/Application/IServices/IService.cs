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
    Task<PagingModel<TResponse>> GetAll(int? pageSize, int? pageIndex, string? sort, CancellationToken ct);
    Task<OperationResult<TResponse>> GetBy(int id, CancellationToken ct);
    Task<OperationResult<TResponse>> CreateAsync(TRequest request, CancellationToken ct);
    Task<OperationResult<bool>> UpdateAsync(int id, TRequest request, CancellationToken ct);
    Task<OperationResult<bool>> DeleteAsync(int id, CancellationToken ct);
}