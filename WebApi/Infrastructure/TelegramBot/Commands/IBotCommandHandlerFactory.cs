using Telegram.Bot.Types;

namespace Infrastructure.TelegramBot.Commands;

public interface IBotCommandHandlerFactory
{
    IBotCommandHandler? CreateCommandHandler(Message commandText);
}
