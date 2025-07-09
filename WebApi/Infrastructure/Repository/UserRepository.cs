using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Domain.Database.Models;
using ServiceDesk.Infrastructure.Database;
using ServiceDesk.Application.IRepository;

namespace ServiceDesk.Infrastructure.Repository;

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
        return await query.Where(x => x.UserName == login).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByEmail(string email)
    {
        var query = GetQuery();
        return await query.Where(x => x.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByLoginOrEmail(string login, string email)
    {
        var query = GetQuery();
        return await query.Where(x => x.UserName == login || x.Email == email)
            .FirstOrDefaultAsync();
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
}
