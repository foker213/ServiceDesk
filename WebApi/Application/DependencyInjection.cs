using Microsoft.Extensions.DependencyInjection;
using ServiceDesk.Application.IServices;
using ServiceDesk.Application.Services;

namespace ServiceDesk.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // configuration services
        services.AddScoped<IExternalUserService, ExternalUserService>();
        services.AddScoped<IRequestService, RequestService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}