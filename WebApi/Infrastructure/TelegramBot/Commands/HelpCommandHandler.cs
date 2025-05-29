using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.Commands;

public class HelpCommandHandler : IBotCommandHandler
{
    private readonly ITelegramBotClient _botClient;

    public string Command => "Помощь";

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
                InlineKeyboardButton.WithUrl("Наш сайт", "https://example.com"),
                InlineKeyboardButton.WithCallbackData("FAQ", "show_faq")
            }
        });

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "Помощь", "Контакты" },
            new KeyboardButton[] { "Настройки" }
        })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId: chatId,
            text: "📚 <b>Справка по боту</b>\n\n" +
                  "Здесь вы можете получить помощь по работе с ботом.\n" +
                  "Выберите нужный раздел:",
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
