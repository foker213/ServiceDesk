using Telegram.Bot.Types;

namespace ServiceDesk.TelegramBot.Commands.ICommand;

public interface IBotCommandHandler
{
    string Command { get; }
    Task HandleCommandAsync(long chatId, Message text, CancellationToken ct);
}
