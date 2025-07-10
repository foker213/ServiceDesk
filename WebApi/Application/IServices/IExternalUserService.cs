using ServiceDesk.Contracts.ExternalUser;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.IServices;

public interface IExternalUserService
{
    Task Create(ExternalUserCommonRequested externalUser);
    Task Update(ExternalUserCommonRequested externalUser);
    Task Delete(int id);
    Task<ExternalUserCommonRequested> GetByEmail(string email);
    Task<ExternalUserCommonRequested> GetByPhone(string phone);
}
