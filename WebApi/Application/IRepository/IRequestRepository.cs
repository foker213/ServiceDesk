using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.IRepository;

public interface IRequestRepository : IRepository<Request>
{
    Task<List<Request>> GetAll(int limit = 10, int offset = 0, string? sort = null, string dictionaryType = "");
    Task<List<Request>> GetByExternalUserId(int externalUserId);
}
