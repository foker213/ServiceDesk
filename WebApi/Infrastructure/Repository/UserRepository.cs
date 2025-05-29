using Application.Repository;
using Domain.DataBase.Models;
using Infrastructure.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

internal sealed class UserRepository(
    UserManager<User> userManager,
    IUserStore<User> userStore,
    ServiceDeskDbContext db,
    TimeProvider tp
) : Repository<User>(db, tp), IUserRepository
{
    public async Task<User?> GetByLogin(string login)
    {
        var query = GetQuery();
        return await query
            .Where(e => e.UserName == login)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetByEmail(string email)
    {
        var query = GetQuery();
        return await query
            .Where(e => e.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetByLoginOrEmail(string login, string email)
    {
        var query = GetQuery();
        return await query
            .Where(e => e.UserName == login || e.Email == email)
            .FirstOrDefaultAsync();
    }

    public override async Task Create(User user)
    {
        var emailStore = (IUserEmailStore<User>)userStore;
        await userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
        await emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);
        user.CreatedAt = _timeProvider.GetUtcNow().UtcDateTime;

        await userManager.CreateAsync(user);
    }

    public async Task Create(User user, string password)
    {
        var emailStore = (IUserEmailStore<User>)userStore;
        await userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
        await emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);
        user.CreatedAt = _timeProvider.GetUtcNow().UtcDateTime;

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
            return;

        var error = result.Errors.FirstOrDefault();
        throw new Exception(error?.Description);
    }

    public override async Task Update(User user)
    {
        var emailStore = (IUserEmailStore<User>)userStore;
        await userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
        await emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);
        await base.Update(user);
    }
}
