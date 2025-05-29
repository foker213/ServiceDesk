using Domain.DataBase.Models;

namespace Application.Repository;

public interface IUserRepository
{
    Task<User?> GetByLogin(string login);
    Task<User?> GetByEmail(string email);
    Task<User?> GetByLoginOrEmail(string login, string email);
}
