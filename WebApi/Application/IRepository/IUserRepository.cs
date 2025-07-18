using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.IRepository;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByLogin(string login, CancellationToken ct);
    Task<User?> GetByEmail(string email, CancellationToken ct);
    Task<User?> GetByLoginOrEmail(string login, string email, CancellationToken ct);
    Task Create(User user, string password, CancellationToken ct);
}
