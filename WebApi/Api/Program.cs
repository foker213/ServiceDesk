using Api;
using Domain.DataBase.Models;
using Infrastructure;
using Infrastructure.DataBase;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApi()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var applicationDbContext = scope.ServiceProvider.GetRequiredService<ServiceDeskDbContext>();
    applicationDbContext.Database.EnsureCreated();
}

app.MapGroup("/account").MapIdentityApi<User>();

app.MapControllers();
app.UseAuthorization();

app.Run();