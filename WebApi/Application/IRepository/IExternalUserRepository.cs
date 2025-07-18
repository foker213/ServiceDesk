using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.IRepository;

public interface IExternalUserRepository : IRepository<ExternalUser>
{
    Task<ExternalUser?> GetByPhone(string number, CancellationToken ct);
    Task<ExternalUser?> GetByEmail(string email, CancellationToken ct);
}