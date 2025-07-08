using Domain.DataBase.Models;

namespace Infrastructure.Repository.IRepository;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByLogin(string login);
    Task<User?> GetByEmail(string email);
    Task<User?> GetByLoginOrEmail(string login, string email);
    Task Create(User user, string password);
}
