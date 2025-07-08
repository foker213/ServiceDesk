using Domain.DataBase.Models;

namespace Infrastructure.Repository.IRepository;

public interface IRequestRepository : IRepository<Request>
{
    Task<List<Request>> GetAll(int limit = 10, int offset = 0, string? sort = null, string dictionaryType);
}
