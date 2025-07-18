using MapsterMapper;
using ServiceDesk.Application.IRepository;
using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts.Chat;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.Services;

public class ChatService(
    IChatRepository repository,
    IMapper mapper,
    TimeProvider tp
) : Service<ChatCommonRequest, ChatResponse, IChatRepository, Chat>(repository, mapper, tp), 
    IChatService
{
    public async Task<int> GetId(long telegramChatId, bool noTracking, CancellationToken ct) =>
        await repository.GetId(telegramChatId, noTracking);
}
