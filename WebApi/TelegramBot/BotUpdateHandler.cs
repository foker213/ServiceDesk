using ServiceDesk.TelegramBot.Commands;
using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.Factory;
using ServiceDesk.TelegramBot.State;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace ServiceDesk.Infrastructure.TelegramBot;

public class BotUpdateHandler : IUpdateHandler
{
    private readonly IBotCommandHandlerFactory _commandHandlerFactory;
    private readonly IUserStateService _userStateService;

    public BotUpdateHandler(IBotCommandHandlerFactory commandHandlerFactory, IUserStateService userStateService)
    {
        _commandHandlerFactory = commandHandlerFactory;
        _userStateService = userStateService;
    }
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } && update.CallbackQuery is not { })
            return;

        Message message = update.Message is not null ? update.Message! : update.CallbackQuery!.Message!;

        long chatId = message.Chat.Id;
        string text = update.Message?.Text ?? update.CallbackQuery?.Data ?? string.Empty;
        string? callbackId = update.CallbackQuery?.Id;

        IBotCommandHandler commandHandler = _commandHandlerFactory.CreateCommandHandler(text);

        var userState = _userStateService.GetUserState(chatId);
        if (userState != UserState.None && commandHandler is UnknownCommandHandler)
        {
            var inputHandler = _commandHandlerFactory.CreateInputHandler(chatId);
            
            if(inputHandler is not null)
            {
                await inputHandler.HandleInputAsync(chatId, text, cancellationToken);
                return;
            }
        }

        await commandHandler.HandleCommandAsync(chatId, text, callbackId, cancellationToken);
        return;
    }
}
