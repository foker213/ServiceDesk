using ServiceDesk.Application.IRepository;
using ServiceDesk.Contracts.ExternalUser;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.IServices;

public interface IExternalUserService : IService<ExternalUserCommonRequest, ExternalUserResponse, IExternalUserRepository, ExternalUser>
{
    Task<ExternalUserResponse> GetByEmail(string email);
    Task<ExternalUserResponse> GetByPhone(string phone);
}
