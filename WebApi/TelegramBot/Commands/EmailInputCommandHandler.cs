using ServiceDesk.TelegramBot.Commands.ICommand;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ServiceDesk.TelegramBot.Commands;

public class EmailInputCommandHandler : IBotCommandHandler, IBotCallbackQueryHandler
{
    private readonly ITelegramBotClient _botClient;

    public string Command => BotCommands.EMAIL_INPUT;

    public EmailInputCommandHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task HandleCommandAsync(long chatId, Message text, CancellationToken ct)
    {
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
    }

    public async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken ct)
    {
        var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { BotCommands.BACK },
                new KeyboardButton[] { BotCommands.PHONE_INPUT, BotCommands.HELP }
            })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
                    chatId: callbackQuery.Message!.Chat.Id,
                    text: "Введите email в формате example@domain.com",
                    replyMarkup: keyboard,
                    cancellationToken: ct);

        await _botClient.AnswerCallbackQuery(
            callbackQueryId: callbackQuery.Id,
            text: string.Empty);
    }
}
