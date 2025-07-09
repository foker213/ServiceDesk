using ServiceDesk.TelegramBot.Commands.ICommand;
using Telegram.Bot.Types;

namespace ServiceDesk.TelegramBot.Factory;

public interface IBotCommandHandlerFactory
{
    IBotCommandHandler CreateCommandHandler(Message commandText);
    IBotCallbackQueryHandler? CreateCallbackQueryHandler(string commandText);
}
