using Domain.DataBase.Models;
using Infrastructure.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Infrastructure.TelegramBot;
using Infrastructure.TelegramBot.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Application.Repository;
using Infrastructure.Repository;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //Регистрация DbContext
        services.AddDbContext<ServiceDeskDbContext>(options =>
            options.UseNpgsql(configuration["DbContext:ConnectionString"]));

        services.AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<ServiceDeskDbContext>()
            .AddDefaultTokenProviders();

        services.AddSingleton<ITelegramBotClient>(_ =>
            new TelegramBotClient(configuration["TelegramBot:Token"]!));

        services.AddScoped<BotUpdateHandler>();
        services.AddScoped<HelpCommandHandler>();

        services.AddTransient<IBotCommandHandler, StartCommandHandler>();
        services.AddTransient<IBotCommandHandler, HelpCommandHandler>();

        services.AddSingleton<IBotCommandHandlerFactory, BotCommandHandlerFactory>();

        services.AddScoped<IUserRepository, UserRepository>();

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
