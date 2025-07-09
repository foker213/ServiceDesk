using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceDesk.Infrastructure.TelegramBot;
using ServiceDesk.TelegramBot.Commands;
using Telegram.Bot;

namespace ServiceDesk.TelegramBot;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITelegramBotClient>(_ =>
            new TelegramBotClient(configuration["TelegramBot:Token"]!));

        services.AddScoped<BotUpdateHandler>();
        services.AddScoped<HelpCommandHandler>();

        services.AddTransient<IBotCommandHandler, StartCommandHandler>();
        services.AddTransient<IBotCommandHandler, HelpCommandHandler>();

        services.AddSingleton<IBotCommandHandlerFactory, BotCommandHandlerFactory>();

        services.AddHostedService(provider =>
        {
            using var scope = provider.CreateScope();
            var updateHandler = scope.ServiceProvider.GetRequiredService<BotUpdateHandler>();
            var botClient = provider.GetRequiredService<ITelegramBotClient>();
            return new BotBackgroundService(botClient, updateHandler);
        });

        return services;
    }
}
