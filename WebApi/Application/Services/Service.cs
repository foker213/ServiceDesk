using Mapster;
using MapsterMapper;
using ServiceDesk.Application.IRepository;
using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts;
using ServiceDesk.Domain.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServiceDesk.Application.Services;

public abstract class Service<TRequest, TResponse, TRepository, TEntity> 
    : IService<TRequest, TResponse, TRepository, TEntity>
    where TRequest : class
    where TResponse : class
    where TRepository : IRepository<TEntity>
    where TEntity : class, IEntity
{
    protected readonly TRepository _repository;
    protected readonly IMapper _mapper;
    protected readonly TimeProvider _timeProvider;

    protected Service(
        TRepository repository,
        IMapper mapper,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _mapper = mapper;
        _timeProvider = timeProvider;
    }

    public virtual async Task<PagingModel<TResponse>> GetAll(
        int? pageSize,
        int? pageIndex,
        string? sort)
    {
        int limit = pageSize ?? 10;
        int offset = ((pageIndex ?? 1) - 1) * limit;

        List<TEntity> entities = await _repository.GetAll(limit, offset, sort);

        return new(entities.Count(), entities.Adapt<List<TResponse>>());
    }

    public virtual async Task<OperationResult<TResponse>> GetBy(int id)
    {
        TEntity? entity = await _repository.GetBy(id);

        if (entity is not null)
            return new() {
                Value = entity.Adapt<TResponse>()
            };

        return new()
        {
            IsError = true,
            ErrorMessage = "Данного объекта не существует"
        };
    }

    public virtual async Task<OperationResult<TResponse>> CreateAsync(TRequest request)
    {
        TEntity? entity = request.Adapt<TEntity>();
        entity.CreatedAt = _timeProvider.GetUtcNow().UtcDateTime;

        await _repository.CreateAsync(entity);

        return new();
    }

    public virtual async Task<OperationResult<bool>> UpdateAsync(int id, TRequest request)
    {
        TEntity? existEntity = await _repository.GetBy(id);
        if (existEntity is null)
            return new()
            {
                IsError = true,
                ErrorMessage = "Данного объекта не существует"
            };

        TEntity entity = request.Adapt(existEntity);
        entity.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;

        await _repository.UpdateAsync(entity);

        return new();
    }

    public virtual async Task<OperationResult<bool>> DeleteAsync(int id)
    {
        TEntity? existEntity = await _repository.GetBy(id);
        if (existEntity is null)
            return new()
            {
                IsError = true,
                ErrorMessage = "Данного объекта не существует"
            };

        await _repository.DeleteAsync(id, existEntity);

        return new();
    }
}