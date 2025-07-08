using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddControllers();

        //Configuration Mapster
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(typeAdapterConfig);
        services.AddScoped<IMapper, ServiceMapper>();

        //Configuration Identity
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
            options.LoginPath = "/Auth/Login";
            options.LogoutPath = "/Auth/Logout";
            options.SlidingExpiration = true;
            options.Cookie.Domain = "localhost";
        });
        return services;
    }
}