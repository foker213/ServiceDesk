using Domain.DataBase.Models;

namespace Infrastructure.Repository.IRepository;

public interface IExternalUser : IRepository<ExternalUser>
{
    Task<ExternalUser> GetByNumber(string number);
    Task<ExternalUser> GetByEmail(string email);
}
