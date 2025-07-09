using ServiceDesk.TelegramBot.Commands.ICommand;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ServiceDesk.TelegramBot.Commands;

public class AlreadyRegisteredCommandHandler : IBotCommandHandler
{
    private readonly ITelegramBotClient _botClient;

    public string Command => BotCommands.ALREADY_REGISTERED;

    public AlreadyRegisteredCommandHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task HandleCommandAsync(long chatId, Message text, CancellationToken ct)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(BotCommands.PHONE_INPUT, BotCommands.PHONE_INPUT),
                InlineKeyboardButton.WithCallbackData(BotCommands.EMAIL_INPUT, BotCommands.EMAIL_INPUT)
            }
        });

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { BotCommands.BACK, BotCommands.HELP }
        })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId: chatId,
            text: "Выберите номер телефона или адрес электронной почты, с которого ранее обращались:",
            replyMarkup: replyKeyboard);

        await _botClient.SendMessage(
            chatId: chatId,
            text: "Дополнительные опции:",
            replyMarkup: inlineKeyboard);
    }
}