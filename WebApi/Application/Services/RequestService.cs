using Mapster;
using ServiceDesk.Application.IRepository;
using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts.Request;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.Services;

public class RequestService : IRequestService
{
    private IRequestRepository _requestRepository;

    public RequestService(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }
    public async Task CreateAsync(RequestCreateModel model) =>
        await _requestRepository.CreateAsync(model.Adapt<Request>());

    public async Task<List<RequestReadModel>> GetByExternalUserId(int externalUserId)
    {
        List<Request> result = await _requestRepository.GetByExternalUserId(externalUserId);
        return result.Adapt<List<RequestReadModel>>();
    }
}