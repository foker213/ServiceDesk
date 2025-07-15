using ServiceDesk.Application.IRepository;
using ServiceDesk.Contracts;
using ServiceDesk.Contracts.Request;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.IServices;

public interface IRequestService : IService<RequestCommonRequest, RequestResponse, IRequestRepository, Request>
{
    Task<List<RequestResponse>> GetByExternalUserId(int externalUserId);
    Task<PagingModel<RequestResponse>> GetAll(int? pageSize, int? pageIndex, string? sort, string dictionaryType = "");
    Task<OperationResult<bool>> UpdateStatusAsync(int id);
}
