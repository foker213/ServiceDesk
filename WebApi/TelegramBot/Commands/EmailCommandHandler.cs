using ServiceDesk.Application.IServices;
using ServiceDesk.Application.Services;
using ServiceDesk.Contracts.ExternalUser;
using ServiceDesk.Contracts.Request;
using ServiceDesk.Domain.Database;
using ServiceDesk.TelegramBot.CommandKeys;
using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.Commands.InputHandlers;
using ServiceDesk.TelegramBot.State;
using System.Numerics;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ServiceDesk.TelegramBot.Commands;

public class EmailCommandHandler : EmailInputHandler, IBotCommandHandler
{
    private readonly IUserStateService _userStateService;
    private readonly IExternalUserService _externalUserService;
    private readonly IRequestService _requestService;

    private readonly ListRequestsCommandHandler _listRequestsCommandHandler;

    public string Command => BotCommands.EMAIL_INPUT;

    public EmailCommandHandler(
        ITelegramBotClient botClient, 
        IUserStateService userStateService, 
        ListRequestsCommandHandler listRequestsCommandHandler,
        IExternalUserService externalUserService,
        IRequestService requestService
    ) 
        : base(botClient)
    {
        _userStateService = userStateService;
        _listRequestsCommandHandler = listRequestsCommandHandler;
        _externalUserService = externalUserService;
        _requestService = requestService;
    }

    public async Task HandleCommandAsync(long chatId, string text, string? callbackId, CancellationToken ct)
    {
        _userStateService.SetUserState(chatId, UserState.WaitingForEmail);

        var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { BotCommands.BACK },
                new KeyboardButton[] { BotCommands.PHONE_INPUT, BotCommands.HELP }
            })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId,
            "Введите email в формате ваш_адрес@domain.com",
            replyMarkup: keyboard,
            cancellationToken: ct);

        if (callbackId is not null)
        {
            await _botClient.AnswerCallbackQuery(callbackId);
        }
    }

    protected override async Task ProcessValidEmailAsync(long chatId, string email, CancellationToken ct)
    {
        string? fullName = _userStateService.GetUserData(chatId, "FullName");

        if (!string.IsNullOrEmpty(fullName))
            await _externalUserService.Create(new()
            {
                FullName = fullName,
                Email = email
            });

        ExternalUserCommonRequested userInfo = await _externalUserService.GetByEmail(email);

        if (userInfo is null)
        {
            await _botClient.SendMessage(
            chatId,
            "Пользователя с данным адресом электронной почты не существует. Пожалуйста, повторите попытку или зарегистрируйтесь.",
            cancellationToken: ct);

            return;
        }

        List<RequestReadModel> requests = await _requestService.GetByExternalUserId(userInfo.UserId);

        IEnumerable<RequestReadModel> filteredRequests = requests.Where(r => r.Status == Convert.ToString(Status.Solved));

        string text = $"{userInfo.FullName}\n{BotCommands.LIST_OLD_REQUESTS}\n";

        if (filteredRequests == null || !filteredRequests.Any())
        {
            text += "Список заявок пуст";
        }
        else
        {

            text += string.Join("\n", filteredRequests.Select(r =>
                $"{r.CreateAt?.ToString("dd.MM.yyyy HH:mm")} - {r.Description}"));
        }

        _userStateService.SetUserData(chatId, "UserId", Convert.ToString(userInfo.UserId));

        await _listRequestsCommandHandler.HandleCommandAsync(chatId, text, null, ct);
    }
}
