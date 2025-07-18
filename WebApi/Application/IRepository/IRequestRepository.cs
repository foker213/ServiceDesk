using ServiceDesk.Contracts;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.IRepository;

public interface IRequestRepository : IRepository<Request>
{
    Task<PagingModel<Request>> GetAll(int limit = 10, int offset = 0, string? sort = null, string dictionaryType = "", CancellationToken ct = default);
    Task<List<Request>> GetByExternalUserId(int externalUserId, CancellationToken ct);
}
