using Telegram.Bot.Types;

namespace ServiceDesk.TelegramBot.Commands;

public interface IBotCommandHandler
{
    string Command { get; }
    Task HandleCommandAsync(long chatId, Message text, CancellationToken ct);
}
