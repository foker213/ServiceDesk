using ServiceDesk.Application.IRepository;
using ServiceDesk.Contracts.ExternalUser;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.IServices;

public interface IExternalUserService : IService<ExternalUserCommonRequest, ExternalUserResponse, IExternalUserRepository, ExternalUser>
{
    Task<ExternalUserResponse> GetByEmail(string email, CancellationToken ct);
    Task<ExternalUserResponse> GetByPhone(string phone, CancellationToken ct);
}
