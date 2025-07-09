using Microsoft.Extensions.DependencyInjection;
using ServiceDesk.TelegramBot.Commands;
using ServiceDesk.TelegramBot.Commands.ICommand;
using Telegram.Bot.Types;

namespace ServiceDesk.TelegramBot.Factory;

public class BotCommandHandlerFactory : IBotCommandHandlerFactory
{
    private readonly IEnumerable<IBotCommandHandler> _commandHandlers;
    private readonly IEnumerable<IBotCallbackQueryHandler> _callbackHandlers;
    private readonly IServiceProvider _serviceProvider;

    public BotCommandHandlerFactory(IEnumerable<IBotCommandHandler> commandHandlers, IEnumerable<IBotCallbackQueryHandler> callbackQueryHandler, IServiceProvider serviceProvider)
    {
        _commandHandlers = commandHandlers;
        _callbackHandlers = callbackQueryHandler;
        _serviceProvider = serviceProvider;
    }

    public IBotCommandHandler CreateCommandHandler(Message command)
    {
        IBotCommandHandler? commandHandler = _commandHandlers.FirstOrDefault(h => h.Command.Equals(command.Text, StringComparison.CurrentCultureIgnoreCase));

        if (command.Text == "/start")
            return _serviceProvider.GetRequiredService<StartCommandHandler>();
        else if(commandHandler is null)
            return _serviceProvider.GetRequiredService<UnknownCommandHandler>();

        return commandHandler!;
    }

    public IBotCallbackQueryHandler? CreateCallbackQueryHandler(string command)
    {
        return _callbackHandlers.FirstOrDefault(h => h.Command.Equals(command, StringComparison.CurrentCultureIgnoreCase));
    }
}
