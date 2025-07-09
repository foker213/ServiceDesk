using ServiceDesk.TelegramBot.Commands.ICommand;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ServiceDesk.TelegramBot.Commands;

public class HelpCommandHandler : IBotCommandHandler
{
    private readonly ITelegramBotClient _botClient;

    public string Command => BotCommands.HELP;

    public HelpCommandHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task HandleCommandAsync(long chatId, Message text, CancellationToken ct)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithUrl("Сайт поддержки", "https://example.com")
            }
        });

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { BotCommands.BACK, BotCommands.Conacts }
        })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId: chatId,
            text: "📚 <b>Справка по боту</b>\n\n" +
                  "Здесь вы можете получить помощь по работе с ботом.\n",
            parseMode: ParseMode.Html,
            replyMarkup: replyKeyboard,
            cancellationToken: ct);

        await _botClient.SendMessage(
            chatId: chatId,
            text: "Дополнительные опции:",
            replyMarkup: inlineKeyboard,
            cancellationToken: ct);
    }
}
