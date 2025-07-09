using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.Factory;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace ServiceDesk.Infrastructure.TelegramBot;

public class BotUpdateHandler : IUpdateHandler
{
    private readonly IBotCommandHandlerFactory _commandHandlerFactory;
    public BotUpdateHandler(IBotCommandHandlerFactory commandHandlerFactory)
    {
        _commandHandlerFactory = commandHandlerFactory;
    }
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.CallbackQuery != null)
        {
            IBotCallbackQueryHandler? callbackHandler = _commandHandlerFactory.CreateCallbackQueryHandler(update.CallbackQuery.Data!);

            if (callbackHandler != null) {
                await callbackHandler.HandleCallbackQueryAsync(update.CallbackQuery, cancellationToken);
                return;
            }

            return;
        }

        if (update.Message is not { } message || update.Message is not { } text)
            return;


        long chatId = message.Chat.Id;

        IBotCommandHandler commandHandler = _commandHandlerFactory.CreateCommandHandler(text);

        await commandHandler.HandleCommandAsync(chatId, text, cancellationToken);
        return;
    }
}
