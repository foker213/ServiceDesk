using Mapster;
using MapsterMapper;
using ServiceDesk.Application.IRepository;
using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts;
using ServiceDesk.Contracts.Request;
using ServiceDesk.Domain.Database;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.Services;

public class RequestService(
    IRequestRepository repository, 
    IMapper mapper, 
    TimeProvider tp,
    ICurrentUserService userService
) : Service<RequestCommonRequest, RequestResponse, IRequestRepository, Request>(repository, mapper, tp), 
    IRequestService
{
    private readonly ICurrentUserService _userService = userService;

    public async Task<PagingModel<RequestResponse>> GetAll(int? pageSize, int? pageIndex, string? sort = null, string dictionaryType = "")
    {
        int limit = pageSize ?? 10;
        int offset = ((pageIndex ?? 1) - 1) * limit;

        PagingModel<Request> result = await repository.GetAll(limit, offset, sort, dictionaryType);

        return result.Adapt<PagingModel<RequestResponse>>();
    }

    public async Task<List<RequestResponse>> GetByExternalUserId(int externalUserId)
    {
        List<Request> result = await repository.GetByExternalUserId(externalUserId);

        return result.Adapt<List<RequestResponse>>();
    }

    public async Task<OperationResult<bool>> UpdateStatusAsync(int id)
    {
        var existRequest = await repository.GetBy(id);
        if (existRequest == null)
            return new()
            {
                IsError = true
            };

        if (existRequest.Status == Status.NotAssigned)
        {
            existRequest.Status = Status.AtWork;
            existRequest.UserId = _userService.UserId;
            existRequest.DateStartRequest = _timeProvider.GetUtcNow().UtcDateTime;
        }
        else
        {
            existRequest.Status = Status.Solved;
            existRequest.DateEndRequest = _timeProvider.GetUtcNow().UtcDateTime;
        }

        await repository.UpdateAsync(existRequest);

        return new();
    }
}