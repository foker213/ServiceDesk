using ServiceDesk.TelegramBot.CommandKeys;
using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.Commands.InputHandlers;
using ServiceDesk.TelegramBot.State;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ServiceDesk.TelegramBot.Commands;

public class StartCommandHandler : FullNameInputHandler, IBotCommandHandler
{
    private readonly IUserStateService _userStateService;

    private readonly EmailCommandHandler _emailCommandHandler;

    public string Command => BotCommands.BACK;

    public StartCommandHandler(ITelegramBotClient botClient, IUserStateService userStateService, EmailCommandHandler emailCommandHandler) : base (botClient)
    {
        _userStateService = userStateService;
        _emailCommandHandler = emailCommandHandler;
    }

    public async Task HandleCommandAsync(long chatId, string text, string? callbackId, CancellationToken ct)
    {
        _userStateService.SetUserState(chatId, UserState.WaitingForFullName);

        var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { BotCommands.ALREADY_REGISTERED },
                new KeyboardButton[] { BotCommands.HELP, BotCommands.Conacts }
            })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId,
            "Здравствуйте!👋 \nЯ бот технической поддержки. Если это ваше первое обращение, тогда напишите ваше ФИО, в формате 'Денисов Михаил Юрьевич'.",
            replyMarkup: keyboard,
            cancellationToken: ct);
    }

    protected override async Task ProcessValidFullNameAsync(long chatId, string fullName, CancellationToken ct)
    {
        await _emailCommandHandler.HandleCommandAsync(chatId, BotCommands.EMAIL_INPUT, null, ct);
    }
}
