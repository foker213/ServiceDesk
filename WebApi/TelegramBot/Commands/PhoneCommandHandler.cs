using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts.ExternalUser;
using ServiceDesk.Contracts.Request;
using ServiceDesk.Domain.Database;
using ServiceDesk.TelegramBot.CommandKeys;
using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.Commands.InputHandlers;
using ServiceDesk.TelegramBot.State;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ServiceDesk.TelegramBot.Commands;

public class PhoneCommandHandler : PhoneInputHandler, IBotCommandHandler
{
    private readonly IUserStateService _userStateService;
    private readonly IExternalUserService _externalUserService;
    private readonly IRequestService _requestService;

    private readonly ListRequestsCommandHandler _listRequestsCommandHandler;

    public string Command => BotCommands.PHONE_INPUT;

    public PhoneCommandHandler(
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
        _userStateService.SetUserState(chatId, UserState.WaitingForPhone);

        var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { BotCommands.BACK },
                new KeyboardButton[] { BotCommands.EMAIL_INPUT, BotCommands.HELP }
            })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId,
            "Введите номер телефона в формате +7-XXX-XXX-XX-XX",
            replyMarkup: keyboard,
            cancellationToken: ct);

        if (callbackId is not null)
        {
            await _botClient.AnswerCallbackQuery(callbackId);
        }
    }

    protected override async Task ProcessValidPhoneAsync(long chatId, string phone, CancellationToken ct)
    {
        string? fullName = _userStateService.GetUserData(chatId, "FullName");

        if (!string.IsNullOrEmpty(fullName))
            await _externalUserService.Create(new()
            {
                FullName = fullName,
                Phone = phone
            });

        ExternalUserCommonRequested? userInfo = await _externalUserService.GetByPhone(phone);

        if(userInfo is null)
        {
            await _botClient.SendMessage(
            chatId,
            "Пользователя с данным номером телефона не существует. Пожалуйста, повторите попытку или зарегистрируйтесь.",
            cancellationToken: ct);

            return;
        }

        List<RequestReadModel> requests = await _requestService.GetByExternalUserId(userInfo.UserId);

        string text = $"{userInfo.FullName}\n{BotCommands.LIST_OLD_REQUESTS}\n";

        if (requests == null || !requests.Any())
        {
            text += "Список заявок пуст";
        }
        else
        {
            var filteredRequests = requests.Where(r => r.Status == Convert.ToString(Status.Solved));

            text += string.Join("\n", filteredRequests.Select(r =>
                $"{r.CreatedAt.ToString("dd.MM.yyyy HH:mm")} - {r.Description}"));
        }

        _userStateService.SetUserData(chatId, "UserId", Convert.ToString(userInfo.UserId));

        await _listRequestsCommandHandler.HandleCommandAsync(chatId, text, null, ct);
    }
}
