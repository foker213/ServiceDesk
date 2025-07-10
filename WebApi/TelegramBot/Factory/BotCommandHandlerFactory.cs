using Microsoft.Extensions.DependencyInjection;
using ServiceDesk.TelegramBot.Commands;
using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.Commands.InputHandlers.IInputHandler;
using ServiceDesk.TelegramBot.State;

namespace ServiceDesk.TelegramBot.Factory;

public class BotCommandHandlerFactory : IBotCommandHandlerFactory
{
    private readonly IEnumerable<IBotCommandHandler> _commandHandlers;
    private readonly IUserStateService _userStateService;
    private readonly StartCommandHandler _startHandler;
    private readonly UnknownCommandHandler _unknownHandler;
    private readonly PhoneCommandHandler _phoneHandler;
    private readonly EmailCommandHandler _emailHandler;

    public BotCommandHandlerFactory(
        IEnumerable<IBotCommandHandler> commandHandlers,
        IUserStateService userStateService,
        StartCommandHandler startCommandHandler,
        UnknownCommandHandler unknownCommandHandler,
        PhoneCommandHandler phoneCommandHandler,
        EmailCommandHandler emailCommandHandler
    )
    {
        _commandHandlers = commandHandlers;
        _userStateService = userStateService;
        _startHandler = startCommandHandler;
        _unknownHandler = unknownCommandHandler;
        _phoneHandler = phoneCommandHandler;
        _emailHandler = emailCommandHandler;
    }

    public IBotCommandHandler CreateCommandHandler(string commandText)
    {
        IBotCommandHandler? commandHandler = _commandHandlers.FirstOrDefault(h => h.Command.Equals(commandText, StringComparison.CurrentCultureIgnoreCase));

        if (commandText == "/start")
            return _startHandler;

        return commandHandler ?? _unknownHandler;
    }

    public IInputDataHandler? CreateInputHandler(long chatId)
    {
        var state = _userStateService.GetUserState(chatId);

        return state switch
        {
            UserState.WaitingForFullName => _startHandler,
            UserState.WaitingForPhone => _phoneHandler,
            UserState.WaitingForEmail => _emailHandler,
            _ => null
        };
    }
}
