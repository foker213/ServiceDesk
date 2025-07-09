using Telegram.Bot.Types;

namespace ServiceDesk.TelegramBot.Commands.ICommand;

public interface IBotCallbackQueryHandler
{
    string Command { get; }
    Task HandleCallbackQueryAsync(CallbackQuery callback, CancellationToken ct);
}
