using Infrastructure;
using ServiceDesk.Api;
using ServiceDesk.Application;
using ServiceDesk.Domain.Database.Models;
using ServiceDesk.Infrastructure.Database;
using ServiceDesk.TelegramBot;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApi()
    .AddApplication()
    .AddTelegramBot(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

using (var scope = app.Services.CreateScope())
{
    var applicationDbContext = scope.ServiceProvider.GetRequiredService<ServiceDeskDbContext>();
    applicationDbContext.Database.EnsureCreated();
}

app.MapGroup("/account").MapIdentityApi<User>();

app.MapControllers();
app.UseAuthorization();

app.Run();