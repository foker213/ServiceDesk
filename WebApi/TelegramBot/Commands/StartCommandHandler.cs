using ServiceDesk.TelegramBot.Commands.ICommand;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ServiceDesk.TelegramBot.Commands;

public class StartCommandHandler : IBotCommandHandler
{
    private readonly ITelegramBotClient _botClient;

    public string Command => BotCommands.BACK;

    public StartCommandHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task HandleCommandAsync(long chatId, Message text, CancellationToken ct)
    {
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
}
