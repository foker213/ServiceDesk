using ServiceDesk.TelegramBot.Commands.ICommand;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ServiceDesk.TelegramBot.Commands;

public class ContactCommandHandler : IBotCommandHandler
{
    private readonly ITelegramBotClient _botClient;

    public string Command => BotCommands.Conacts;

    public ContactCommandHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task HandleCommandAsync(long chatId, Message text, CancellationToken ct)
    {
        var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { BotCommands.BACK, BotCommands.HELP }
            })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId,
            "Контакты разработчика: \n📨 Электронная почта: zibulski.yandex@gmail.com\n📞 Номер телефона: *Скрыт по соображениям безопасности*",
            replyMarkup: keyboard,
            cancellationToken: ct);
    }
}