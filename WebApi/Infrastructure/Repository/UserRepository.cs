using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Domain.Database.Models;
using ServiceDesk.Infrastructure.Database;
using ServiceDesk.Application.IRepository;

namespace ServiceDesk.Infrastructure.Repository;

internal sealed class UserRepository(
    UserManager<User> userManager,
    IUserStore<User> userStore,
    ServiceDeskDbContext db
) : Repository<User>(db), IUserRepository
{
    public async Task<User?> GetByLogin(string login, CancellationToken ct)
    {
        var query = GetQuery();
        return await query.Where(x => x.UserName == login).FirstOrDefaultAsync(ct);
    }

    public async Task<User?> GetByEmail(string email, CancellationToken ct)
    {
        var query = GetQuery();
        return await query.Where(x => x.Email == email).FirstOrDefaultAsync(ct);
    }

    public async Task<User?> GetByLoginOrEmail(string login, string email, CancellationToken ct)
    {
        var query = GetQuery();
        return await query.Where(x => x.UserName == login || x.Email == email)
            .FirstOrDefaultAsync(ct);
    }

    public async Task Create(User user, string password, CancellationToken ct)
    {
        var emailStore = (IUserEmailStore<User>)userStore;
        await userStore.SetUserNameAsync(user, user.UserName, ct);
        await emailStore.SetEmailAsync(user, user.Email, ct);

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
            return;

        var error = result.Errors.FirstOrDefault();
        throw new Exception(error?.Description);
    }
}
