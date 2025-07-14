using ServiceDesk.Contracts;
using ServiceDesk.Contracts.Request;

namespace ServiceDesk.Application.IServices;

public interface IRequestService
{
    Task CreateAsync(RequestCreateModel model);
    Task<List<RequestReadModel>> GetByExternalUserId(int externalUserId);
    Task<PagingModel<RequestReadModel>> GetAll(int limit = 10, int offset = 0, string? sort = null, string dictionaryType = "");
    Task<RequestReadModel> GetBy(int id);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(int id, RequestChangeModel request);
    Task<bool> UpdateStatusAsync(int id);
}
