using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts.ExternalUser;
using ServiceDesk.TelegramBot.CommandKeys;
using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.Commands.InputHandlers;
using ServiceDesk.TelegramBot.State;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ServiceDesk.TelegramBot.Commands;

public class PhoneCommandHandler : PhoneInputHandler, IBotCommandHandler
{
    private readonly IUserStateService _userStateService;
    private readonly IExternalUserService _externalUserService;

    private readonly ListRequestsCommandHandler _listRequestsCommandHandler;

    public string Command => BotCommands.PHONE_INPUT;

    public PhoneCommandHandler(
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
        _userStateService.SetUserState(chatId, UserState.WaitingForPhone);

        var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { BotCommands.BACK },
                new KeyboardButton[] { BotCommands.EMAIL_INPUT, BotCommands.HELP }
            })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId,
            "Введите номер телефона в формате +7-XXX-XXX-XX-XX",
            replyMarkup: keyboard,
            cancellationToken: ct);

        if (callbackId is not null)
        {
            await _botClient.AnswerCallbackQuery(callbackId);
        }
    }

    protected override async Task ProcessValidPhoneAsync(long chatId, string phone, CancellationToken ct)
    {
        string? fullName = _userStateService.GetUserData(chatId, "FullName");

        if (fullName is not null)
            await _externalUserService.Create(new()
            {
                FullName = fullName,
                Phone = phone
            });

        ExternalUserCommonRequested userInfo = await _externalUserService.GetByPhone(phone);

        string text = $@"{userInfo.FullName}
                        {BotCommands.LIST_OLD_REQUESTS}
                        Список заявок пуст";

        await _listRequestsCommandHandler.HandleCommandAsync(chatId, text, null, ct);
    }
}
