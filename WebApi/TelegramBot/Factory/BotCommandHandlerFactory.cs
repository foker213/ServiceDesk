using Microsoft.Extensions.DependencyInjection;
using ServiceDesk.TelegramBot.Commands;
using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.Commands.InputHandlers.IInputHandler;
using ServiceDesk.TelegramBot.State;

namespace ServiceDesk.TelegramBot.Factory;

public class BotCommandHandlerFactory : IBotCommandHandlerFactory
{
    private readonly IEnumerable<IBotCommandHandler> _commandHandlers;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserStateService _userStateService;

    public BotCommandHandlerFactory(IEnumerable<IBotCommandHandler> commandHandlers, IServiceProvider serviceProvider, IUserStateService userStateService)
    {
        _commandHandlers = commandHandlers;
        _serviceProvider = serviceProvider;
        _userStateService = userStateService;
    }

    public IBotCommandHandler CreateCommandHandler(string commandText)
    {
        IBotCommandHandler? commandHandler = _commandHandlers.FirstOrDefault(h => h.Command.Equals(commandText, StringComparison.CurrentCultureIgnoreCase));

        if (commandText == "/start")
            return _serviceProvider.GetRequiredService<StartCommandHandler>();
        else if(commandHandler is null)
            return _serviceProvider.GetRequiredService<UnknownCommandHandler>();

        return commandHandler!;
    }

    public IInputDataHandler? CreateInputHandler(long chatId)
    {
        var state = _userStateService.GetUserState(chatId);

        return state switch
        {
            UserState.WaitingForFullName => _serviceProvider.GetRequiredService<StartCommandHandler>(),
            UserState.WaitingForPhone => _serviceProvider.GetRequiredService<PhoneCommandHandler>(),
            UserState.WaitingForEmail => _serviceProvider.GetRequiredService<EmailCommandHandler>(),
            _ => null
        };
    }
}
