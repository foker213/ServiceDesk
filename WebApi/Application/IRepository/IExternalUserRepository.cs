using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.IRepository;

public interface IExternalUserRepository : IRepository<ExternalUser>
{
    Task<ExternalUser?> GetByNumber(string number);
    Task<ExternalUser?> GetByEmail(string email);
}
