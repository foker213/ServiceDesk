using ServiceDesk.Application.IRepository;
using ServiceDesk.Contracts.User;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.IServices;

public interface IUserService : IService<UserCommonRequest, UserResponse, IUserRepository, User>
{
    Task<User?> GetByLogin(string userName);
}
