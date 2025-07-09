using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceDesk.Infrastructure.TelegramBot;
using ServiceDesk.TelegramBot.Commands;
using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.Factory;
using Telegram.Bot;

namespace ServiceDesk.TelegramBot;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITelegramBotClient>(_ =>
            new TelegramBotClient(configuration["TelegramBot:Token"]!));

        services.AddScoped<BotUpdateHandler>();
        services.AddTransient<StartCommandHandler>();
        services.AddTransient<UnknownCommandHandler>();

        services.AddTransient<IBotCommandHandler, StartCommandHandler>();
        services.AddTransient<IBotCommandHandler, HelpCommandHandler>();
        services.AddTransient<IBotCommandHandler, ContactCommandHandler>();
        services.AddTransient<IBotCommandHandler, AlreadyRegisteredCommandHandler>();
        services.AddTransient<IBotCommandHandler, PhoneInputCommandHandler>();
        services.AddTransient<IBotCommandHandler, EmailInputCommandHandler>();
        services.AddTransient<IBotCommandHandler, UnknownCommandHandler>();

        services.AddTransient<IBotCallbackQueryHandler, PhoneInputCommandHandler>();
        services.AddTransient<IBotCallbackQueryHandler, EmailInputCommandHandler>();

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
