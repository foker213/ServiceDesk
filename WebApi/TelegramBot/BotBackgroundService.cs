using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceDesk.Infrastructure.TelegramBot;

public class BotBackgroundService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IServiceProvider _serviceProvider;

    public BotBackgroundService(
        ITelegramBotClient botClient,
        IServiceProvider serviceProvider)
    {
        _botClient = botClient;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[] { UpdateType.Message, UpdateType.CallbackQuery }
        };

        while (!ct.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var updateHandler = scope.ServiceProvider.GetRequiredService<BotUpdateHandler>();
                    await _botClient.ReceiveAsync(
                        updateHandler: updateHandler,
                        receiverOptions: receiverOptions,
                        cancellationToken: ct
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await Task.Delay(1000, ct);
            }
        }
    }
}