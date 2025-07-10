using ServiceDesk.TelegramBot.CommandKeys;
using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.State;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ServiceDesk.TelegramBot.Commands;

public class HelpCommandHandler : IBotCommandHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IUserStateService _userStateService;

    public string Command => BotCommands.HELP;

    public HelpCommandHandler(ITelegramBotClient botClient, IUserStateService userStateService)
    {
        _botClient = botClient;
        _userStateService = userStateService;
    }

    public async Task HandleCommandAsync(long chatId, string text, string? callbackId, CancellationToken ct)
    {
        _userStateService.ClearUserState(chatId);

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
                  "Здесь вы можете получить помощь по работе с ботом.\n\nПосле ввода ФИО, можно изменить выбор с привязки по email на привязку по номеру телефона.",
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
