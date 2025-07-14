using Mapster;
using ServiceDesk.Application.IRepository;
using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts.Chat;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;

    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task CreateAsync(ChatInitiated chatInitiated) =>
        await _chatRepository.CreateAsync(chatInitiated.Adapt<Chat>());

    public async Task<int> GetId(long telegramChatId, bool noTracking = false) =>
        await _chatRepository.GetId(telegramChatId, noTracking);
}
