using Telegram.Bot.Types;

namespace ServiceDesk.TelegramBot.Commands;

public interface IBotCommandHandlerFactory
{
    IBotCommandHandler? CreateCommandHandler(Message commandText);
}
