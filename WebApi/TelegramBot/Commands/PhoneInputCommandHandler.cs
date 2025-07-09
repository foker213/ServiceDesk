using ServiceDesk.TelegramBot.Commands.ICommand;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ServiceDesk.TelegramBot.Commands;

public class PhoneInputCommandHandler : IBotCommandHandler, IBotCallbackQueryHandler
{
    private readonly ITelegramBotClient _botClient;

    public string Command => BotCommands.PHONE_INPUT;

    public PhoneInputCommandHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task HandleCommandAsync(long chatId, Message text, CancellationToken ct)
    {
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
    }

    public async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken ct)
    {
        var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { BotCommands.BACK },
                new KeyboardButton[] { BotCommands.EMAIL_INPUT, BotCommands.HELP }
            })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
                    chatId: callbackQuery.Message!.Chat.Id,
                    text: "Введите номер в формате +7-XXX-XXX-XX-XX",
                    replyMarkup: keyboard,
                    cancellationToken: ct);

        await _botClient.AnswerCallbackQuery(
            callbackQueryId: callbackQuery.Id,
            text: string.Empty);
    }
}
