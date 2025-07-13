using ServiceDesk.TelegramBot.Commands.InputHandlers.IInputHandler;
using Telegram.Bot;

namespace ServiceDesk.TelegramBot.Commands.InputHandlers;

public abstract class NewRequestInputHandler : IInputDataHandler
{
    protected readonly ITelegramBotClient _botClient;

    public NewRequestInputHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task HandleInputAsync(long chatId, string inputData, CancellationToken ct)
    {
        if (!IsValidDescription(inputData))
        {
            await _botClient.SendMessage(
                chatId,
                "Некорректное описание проблемы. Опишите вашу проблему повторно",
                cancellationToken: ct);
            return;
        }

        await ProcessValidDescriptionAsync(chatId, inputData, ct);
    }

    private bool IsValidDescription(string description)
    {
        return !string.IsNullOrWhiteSpace(description);
    }

    protected abstract Task ProcessValidDescriptionAsync(long chatId, string description, CancellationToken ct);
}
