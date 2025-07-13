using ServiceDesk.Contracts.ExternalUser;

namespace ServiceDesk.Application.IServices;

public interface IExternalUserService
{
    Task Create(ExternalUserCommonRequested externalUser);
    Task Update(ExternalUserCommonRequested externalUser);
    Task Delete(int id);
    Task<List<ExternalUserCommonRequested>> GetAll(int limit = 10, int offset = 0, string? sort = null, bool noTracking = false);
    Task<ExternalUserCommonRequested> GetByEmail(string email);
    Task<ExternalUserCommonRequested> GetByPhone(string phone);
}
