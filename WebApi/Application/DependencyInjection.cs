using Microsoft.Extensions.DependencyInjection;
using ServiceDesk.Application.IServices;
using ServiceDesk.Application.Services;

namespace ServiceDesk.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IExternalUserService, ExternalUserService>();
        services.AddScoped<IRequestService, RequestService>();

        return services;
    }
}