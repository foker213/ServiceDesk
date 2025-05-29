using Telegram.Bot.Types;

namespace Infrastructure.TelegramBot.Commands;

public interface IBotCommandHandler
{
    string Command { get; }
    Task HandleCommandAsync(long chatId, Message text, CancellationToken ct);
}
