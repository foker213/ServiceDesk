using ServiceDesk.Contracts.Request;

namespace ServiceDesk.Application.IServices;

public interface IRequestService
{
    Task Create(RequestCreateModel model);
    Task<List<RequestReadModel>> GetByExternalUserId(int externalUserId);
}
