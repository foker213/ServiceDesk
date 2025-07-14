using Mapster;
using ServiceDesk.Application.IRepository;
using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts;
using ServiceDesk.Contracts.Request;
using ServiceDesk.Domain.Database;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.Services;

public class RequestService : IRequestService
{
    private readonly IRequestRepository _requestRepository;
    private readonly ICurrentUserService _userService;
    protected readonly TimeProvider _timeProvider;

    public RequestService(IRequestRepository requestRepository, ICurrentUserService userService, TimeProvider timeProvider)
    {
        _requestRepository = requestRepository;
        _userService = userService;
        _timeProvider = timeProvider;
    }
    public async Task CreateAsync(RequestCreateModel model) =>
        await _requestRepository.CreateAsync(model.Adapt<Request>());

    public async Task<bool> DeleteAsync(int id)
    {
        var existRequest = await _requestRepository.GetBy(id);
        if (existRequest == null)
            return false;

        await _requestRepository.DeleteAsync(id);
        return true;
    }

    public async Task<PagingModel<RequestReadModel>> GetAll(int limit = 10, int offset = 0, string? sort = null, string dictionaryType = "")
    {
        PagingModel<Request> result = await _requestRepository.GetAll(limit, offset, sort, dictionaryType);

        return result.Adapt<PagingModel<RequestReadModel>>();
    }

    public async Task<RequestReadModel> GetBy(int id)
    {
        Request? result = await _requestRepository.GetBy(id);

        return result.Adapt<RequestReadModel>();
    }

    public async Task<List<RequestReadModel>> GetByExternalUserId(int externalUserId)
    {
        List<Request> result = await _requestRepository.GetByExternalUserId(externalUserId);
        return result.Adapt<List<RequestReadModel>>();
    }

    public async Task<bool> UpdateAsync(int id, RequestChangeModel request)
    {
        var existRequest = await _requestRepository.GetBy(id);
        if (existRequest == null)
            return false;

        if (existRequest.Description != request.Description)
            existRequest.Description = request.Description;

        await _requestRepository.UpdateAsync(existRequest);
        return true;
    }

    public async Task<bool> UpdateStatusAsync(int id)
    {
        var existRequest = await _requestRepository.GetBy(id);
        if (existRequest == null)
            return false;

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

        await _requestRepository.UpdateAsync(existRequest);
        return true;
    }
}