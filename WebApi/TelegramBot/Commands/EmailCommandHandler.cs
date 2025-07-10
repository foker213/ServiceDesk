using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts.ExternalUser;
using ServiceDesk.TelegramBot.CommandKeys;
using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.Commands.InputHandlers;
using ServiceDesk.TelegramBot.State;
using System.Numerics;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ServiceDesk.TelegramBot.Commands;

public class EmailCommandHandler : EmailInputHandler, IBotCommandHandler
{
    private readonly IUserStateService _userStateService;
    private readonly IExternalUserService _externalUserService;

    private readonly ListRequestsCommandHandler _listRequestsCommandHandler;

    public string Command => BotCommands.EMAIL_INPUT;

    public EmailCommandHandler(
        ITelegramBotClient botClient, 
        IUserStateService userStateService, 
        ListRequestsCommandHandler listRequestsCommandHandler,
        IExternalUserService externalUserService
    ) 
        : base(botClient)
    {
        _userStateService = userStateService;
        _listRequestsCommandHandler = listRequestsCommandHandler;
        _externalUserService = externalUserService;
    }

    public async Task HandleCommandAsync(long chatId, string text, string? callbackId, CancellationToken ct)
    {
        _userStateService.SetUserState(chatId, UserState.WaitingForEmail);

        var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { BotCommands.BACK },
                new KeyboardButton[] { BotCommands.PHONE_INPUT, BotCommands.HELP }
            })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId,
            "Введите email в формате ваш_адрес@domain.com",
            replyMarkup: keyboard,
            cancellationToken: ct);

        if (callbackId is not null)
        {
            await _botClient.AnswerCallbackQuery(callbackId);
        }
    }

    protected override async Task ProcessValidEmailAsync(long chatId, string email, CancellationToken ct)
    {
        string? fullName = _userStateService.GetUserData(chatId, "FullName");

        if (fullName is not null)
            await _externalUserService.Create(new()
            {
                FullName = fullName,
                Email = email
            });

        ExternalUserCommonRequested userInfo = await _externalUserService.GetByEmail(email);

        string text = $@"{userInfo.FullName}
                        {BotCommands.LIST_OLD_REQUESTS}
                        Список заявок пуст";

        await _listRequestsCommandHandler.HandleCommandAsync(chatId, text, null, ct);
    }
}
