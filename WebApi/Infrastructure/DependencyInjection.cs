using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceDesk.Domain.Database.Models;
using ServiceDesk.Infrastructure.Database;
using ServiceDesk.Infrastructure.Repository;
using ServiceDesk.Application.IRepository;
using ServiceDesk.Infrastructure;
using ServiceDesk.Application.IServices;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ServiceDeskDbContext>(options =>
            options.UseNpgsql(configuration["DbContext:ConnectionString"]));

        services.AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<ServiceDeskDbContext>()
            .AddDefaultTokenProviders();

        // configuration repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IChatLineRepository, ChatLineRepository>();
        services.AddScoped<IRequestRepository, RequestRepository>();
        services.AddScoped<IExternalUserRepository, ExternalRepository>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}