using ServiceDesk.TelegramBot.Commands.InputHandlers.IInputHandler;
using System.Text.RegularExpressions;
using Telegram.Bot;

namespace ServiceDesk.TelegramBot.Commands.InputHandlers;

public abstract class FullNameInputHandler : IInputDataHandler
{
    protected readonly ITelegramBotClient _botClient;

    public FullNameInputHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task HandleInputAsync(long chatId, string inputData, CancellationToken ct)
    {
        if (!IsValidFullName(inputData))
        {
            await _botClient.SendMessage(
                chatId,
                "Некорректный формат ФИО. Введите в формате 'Денисов Михаил Юрьевич'.",
                cancellationToken: ct);
            return;
        }

        await ProcessValidFullNameAsync(chatId, inputData, ct);
    }

    private bool IsValidFullName(string fullName)
    {
        string[] parts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3)
            return false;

        foreach (var part in parts)
        {
            if (string.IsNullOrWhiteSpace(part))
                return false;

            if (!Regex.IsMatch(part, @"^\p{L}+$"))
                return false;
        }

        return true;
    }

    protected abstract Task ProcessValidFullNameAsync(long chatId, string fullName, CancellationToken ct);
}