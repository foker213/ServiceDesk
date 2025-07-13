using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceDesk.Infrastructure.TelegramBot;
using ServiceDesk.TelegramBot.Commands;
using ServiceDesk.TelegramBot.Commands.ICommand;
using ServiceDesk.TelegramBot.Commands.InputHandlers.IInputHandler;
using ServiceDesk.TelegramBot.Factory;
using Telegram.Bot;
using ServiceDesk.TelegramBot.State;

namespace ServiceDesk.TelegramBot;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITelegramBotClient>(_ =>
            new TelegramBotClient(configuration["TelegramBot:Token"]!));

        services.AddSingleton<IUserStateService, UserStateService>();

        services.AddScoped<BotUpdateHandler>();
        services.AddScoped<IBotCommandHandlerFactory, BotCommandHandlerFactory>();

        AddCommandHandlers(services);
        AddInputHandlers(services);

        services.AddHostedService(provider =>
        {
            var botClient = provider.GetRequiredService<ITelegramBotClient>();
            var serviceProvider = provider.GetRequiredService<IServiceProvider>();
            return new BotBackgroundService(botClient, serviceProvider);
        });

        return services;
    }

    private static void AddCommandHandlers(IServiceCollection services)
    {
        services.AddTransient<StartCommandHandler>();
        services.AddTransient<UnknownCommandHandler>();
        services.AddTransient<EmailCommandHandler>();
        services.AddTransient<PhoneCommandHandler>();
        services.AddTransient<ListRequestsCommandHandler>();
        services.AddTransient<NewRequestCommandHandler>();

        services.AddTransient<IBotCommandHandler, StartCommandHandler>();
        services.AddTransient<IBotCommandHandler, HelpCommandHandler>();
        services.AddTransient<IBotCommandHandler, ContactCommandHandler>();
        services.AddTransient<IBotCommandHandler, AlreadyRegisteredCommandHandler>();
        services.AddTransient<IBotCommandHandler, PhoneCommandHandler>();
        services.AddTransient<IBotCommandHandler, EmailCommandHandler>();
        services.AddTransient<IBotCommandHandler, NewRequestCommandHandler>();
        services.AddTransient<IBotCommandHandler, UnknownCommandHandler>();
    }

    private static void AddInputHandlers(IServiceCollection services)
    {
        services.AddTransient<IInputDataHandler, StartCommandHandler>();
        services.AddTransient<IInputDataHandler, EmailCommandHandler>();
        services.AddTransient<IInputDataHandler, PhoneCommandHandler>();
        services.AddTransient<IInputDataHandler, NewRequestCommandHandler>();
    }
}
