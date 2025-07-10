using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.Commands.InputHandlers.IInputHandler;
using Telegram.Bot.Types;

namespace ServiceDesk.TelegramBot.Factory;

public interface IBotCommandHandlerFactory
{
    IBotCommandHandler CreateCommandHandler(string commandText);
    IInputDataHandler? CreateInputHandler(long chatId);
}
