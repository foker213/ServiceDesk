using ServiceDesk.TelegramBot.CommandKeys;
using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.State;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;

namespace ServiceDesk.TelegramBot.Commands;

public class ListRequestsCommandHandler : IBotCommandHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IUserStateService _userStateService;

    public string Command => BotCommands.LIST_OLD_REQUESTS;

    public ListRequestsCommandHandler(ITelegramBotClient botClient, IUserStateService userStateService)
    {
        _botClient = botClient;
        _userStateService = userStateService;
    }

    public async Task HandleCommandAsync(long chatId, string text, string? callbackId, CancellationToken ct)
    {
        _userStateService.ClearUserState(chatId);

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { BotCommands.BACK, BotCommands.CREATE_NEW_REQUEST }
        })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId: chatId,
            text: text,
            parseMode: ParseMode.Html,
            replyMarkup: replyKeyboard,
            cancellationToken: ct);
    }
}