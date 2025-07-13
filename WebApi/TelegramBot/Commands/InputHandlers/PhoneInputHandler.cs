using ServiceDesk.TelegramBot.Commands.InputHandlers.IInputHandler;
using System.Text.RegularExpressions;
using Telegram.Bot;

namespace ServiceDesk.TelegramBot.Commands.InputHandlers;

public abstract class PhoneInputHandler : IInputDataHandler
{
    protected readonly ITelegramBotClient _botClient;

    public PhoneInputHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task HandleInputAsync(long chatId, string inputData, CancellationToken ct)
    {
        if (!IsValidPhone(inputData))
        {
            await _botClient.SendMessage(
                chatId,
                "Некорректный номер телефона. Введите в формате +7-XXX-XXX-XX-XX.",
                cancellationToken: ct);
            return;
        }

        await ProcessValidPhoneAsync(chatId, inputData, ct);
    }

    private bool IsValidPhone(string phone) => Regex.IsMatch(phone, @"^\+7-\d{3}-\d{3}-\d{2}-\d{2}$");

    protected abstract Task ProcessValidPhoneAsync(long chatId, string phone, CancellationToken ct);
}