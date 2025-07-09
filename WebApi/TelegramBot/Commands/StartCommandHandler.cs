using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ServiceDesk.TelegramBot.Commands;

public class StartCommandHandler : IBotCommandHandler
{
    private readonly ITelegramBotClient _botClient;

    public string Command => "/start";

    public StartCommandHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task HandleCommandAsync(long chatId, Message text, CancellationToken ct)
    {
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "Помощь" }
        })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId,
            "Здравствуйте!👋 Я бот технической поддержки. Пожалуйста, напишите как я могу к вам обращаться?",
            replyMarkup: replyKeyboard,
            cancellationToken: ct);
    }
}
