using Telegram.Bot.Types;

namespace ServiceDesk.TelegramBot.Commands;

public class BotCommandHandlerFactory : IBotCommandHandlerFactory
{
    private readonly IEnumerable<IBotCommandHandler> _commandHandlers;

    public BotCommandHandlerFactory(IEnumerable<IBotCommandHandler> commandHandlers)
    {
        _commandHandlers = commandHandlers;
    }

    public IBotCommandHandler? CreateCommandHandler(Message commandText)
    {
        return _commandHandlers.FirstOrDefault(h =>
            commandText.Text?.StartsWith(h.Command, StringComparison.OrdinalIgnoreCase) ?? false);
    }
}
