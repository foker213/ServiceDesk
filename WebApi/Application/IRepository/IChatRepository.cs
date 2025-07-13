using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.IRepository;

public interface IChatRepository : IRepository<Chat>
{
    Task<int> GetId(long telegramChatId, bool noTracking = false);
}
