using Mapster;
using ServiceDesk.Application.IRepository;
using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts.ExternalUser;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.Services;

public class ExternalUserService : IExternalUserService
{
    private IExternalUserRepository _externalUserRepository;

    public ExternalUserService(IExternalUserRepository externalUserRepository)
    {
        _externalUserRepository = externalUserRepository;
    }

    public async Task Create(ExternalUserCommonRequested externalUser) =>
        await _externalUserRepository.CreateAsync(externalUser.Adapt<ExternalUser>());

    public async Task Update(ExternalUserCommonRequested externalUser) =>
        await _externalUserRepository.UpdateAsync(externalUser.Adapt<ExternalUser>());

    public async Task Delete(int id) =>
        await _externalUserRepository.DeleteAsync(id);

    public async Task<ExternalUserCommonRequested> GetByEmail(string email)
    {
        ExternalUser? result = await _externalUserRepository.GetByEmail(email);

        return result.Adapt<ExternalUserCommonRequested>();
    }

    public async Task<ExternalUserCommonRequested> GetByPhone(string phone)
    {
        ExternalUser? result = await _externalUserRepository.GetByPhone(phone);

        return result.Adapt<ExternalUserCommonRequested>();
    }
}
