using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts.ExternalUser;
using ServiceDesk.TelegramBot.CommandKeys;
using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.Commands.InputHandlers;
using ServiceDesk.TelegramBot.State;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ServiceDesk.TelegramBot.Commands;

public class NewRequestCommandHandler : NewRequestInputHandler, IBotCommandHandler
{
    private readonly IUserStateService _userStateService;
    private readonly IExternalUserService _externalUserService;
    private readonly IChatService _chatService;
    private readonly IRequestService _requestService;

    public string Command => BotCommands.CREATE_NEW_REQUEST;

    public NewRequestCommandHandler(
        ITelegramBotClient botClient,
        IUserStateService userStateService,
        IExternalUserService externalUserService,
        IChatService chatService,
        IRequestService requestService
    ) : base(botClient)
    {
        _userStateService = userStateService;
        _externalUserService = externalUserService;
        _chatService = chatService;
        _requestService = requestService;
    }

    public async Task HandleCommandAsync(long chatId, string text, string? callbackId, CancellationToken ct)
    {
        _userStateService.SetUserState(chatId, UserState.WaitengForDescription);

        var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { BotCommands.BACK }
            })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId,
            "Опишите вашу проблему следующим сообщением.",
            replyMarkup: keyboard,
            cancellationToken: ct);

        if (callbackId is not null)
        {
            await _botClient.AnswerCallbackQuery(callbackId);
        }
    }

    protected override async Task ProcessValidDescriptionAsync(long telegramChatId, string description, CancellationToken ct)
    {
        string userId = _userStateService.GetUserData(telegramChatId, "UserId");

        await _chatService.CreateAsync(new()
        {
            ExternalUserId = Convert.ToInt32(userId),
            TelegramChatId = telegramChatId
        });

        int chatId = await _chatService.GetId(telegramChatId);

        await _requestService.CreateAsync(new()
        {
            ChatId = chatId,
            Description = description
        });

        string text = $"Ваша заявка успешно создана. Пожалуйста, ожидайте пока вашу заявку примет первый освободившийся специалист.";

        await _botClient.SendMessage(
            chatId,
            text,
            cancellationToken: ct);
    }
}