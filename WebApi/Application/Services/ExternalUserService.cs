using Mapster;
using MapsterMapper;
using ServiceDesk.Application.IRepository;
using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts.ExternalUser;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.Services;

public class ExternalUserService(
    IExternalUserRepository repository,
    IMapper mapper,
    TimeProvider tp
) : Service<ExternalUserCommonRequest, ExternalUserResponse, IExternalUserRepository, ExternalUser>(repository, mapper, tp), 
    IExternalUserService
{
    public async Task<ExternalUserResponse> GetByEmail(string email, CancellationToken ct)
    {
        ExternalUser? result = await repository.GetByEmail(email);

        return result.Adapt<ExternalUserResponse>();
    }

    public async Task<ExternalUserResponse> GetByPhone(string phone, CancellationToken ct)
    {
        ExternalUser? result = await repository.GetByPhone(phone);

        return result.Adapt<ExternalUserResponse>();
    }
}