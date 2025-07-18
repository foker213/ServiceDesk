using ServiceDesk.Application.IRepository;
using ServiceDesk.Contracts.Chat;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.IServices;

public interface IChatService : IService<ChatCommonRequest, ChatResponse, IChatRepository, Chat>
{
    Task<int> GetId(long telegramChatId, bool noTracking, CancellationToken ct);
}
