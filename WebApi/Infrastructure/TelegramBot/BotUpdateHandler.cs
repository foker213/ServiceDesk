using Infrastructure.TelegramBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Infrastructure.TelegramBot;

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
        if (update.Message is not { } message || update.Message is not { } text)
            return;

        var chatId = message.Chat.Id;

        var commandHandler = _commandHandlerFactory.CreateCommandHandler(text);
        if (commandHandler != null)
        {
            await commandHandler.HandleCommandAsync(chatId, text, cancellationToken);
            return;
        }

        // TODO: доработать сохранение в Redis и выбор через switch/case команды.
    }
}
