using ServiceDesk.Contracts.Chat;

namespace ServiceDesk.Application.IServices;

public interface IChatService
{
    Task CreateAsync(ChatInitiated chatInitiated);
    Task<int> GetId(long telegramChatId, bool noTracking = false);
}
