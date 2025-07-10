using ServiceDesk.TelegramBot.Commands.InputHandlers.IInputHandler;
using System.Text.RegularExpressions;
using Telegram.Bot;

namespace ServiceDesk.TelegramBot.Commands.InputHandlers;

public abstract class EmailInputHandler : IInputDataHandler
{
    protected readonly ITelegramBotClient _botClient;

    public EmailInputHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task HandleInputAsync(long chatId, string inputData, CancellationToken ct)
    {
        if (!IsValidEmail(inputData))
        {
            await _botClient.SendMessage(
                chatId,
                "Некорректный email. Введите в формате ваш_адрес@domain.com.",
                cancellationToken: ct);
            return;
        }

        await ProcessValidEmailAsync(chatId, inputData, ct);
    }

    private bool IsValidEmail(string email) => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    protected abstract Task ProcessValidEmailAsync(long chatId, string email, CancellationToken ct);
}