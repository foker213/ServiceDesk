using Microsoft.EntityFrameworkCore;
using ServiceDesk.Application.IRepository;
using ServiceDesk.Domain.Database.Models;
using ServiceDesk.Infrastructure.Database;

namespace ServiceDesk.Infrastructure.Repository;

internal sealed class ExternalRepository(
    ServiceDeskDbContext db
) : Repository<ExternalUser>(db), IExternalUserRepository
{
    public async Task<ExternalUser?> GetByEmail(string email)
    {
        IQueryable<ExternalUser> query = GetQuery();
        return await query.Where(x => x.Email == email).FirstOrDefaultAsync();
    }

    public async Task<ExternalUser?> GetByPhone(string number)
    {
        IQueryable<ExternalUser> query = GetQuery();
        return await query.Where(x => x.NumberPhone == number).FirstOrDefaultAsync();
    }

    public override async Task CreateAsync(ExternalUser user)
    {
        IQueryable<ExternalUser> query = GetQuery();

        if (query.Any(x => (x.NumberPhone != null && x.NumberPhone == user.NumberPhone) || (x.Email != null && x.Email == user.Email)))
            return;

        DbSet.Add(user);
        await _db.SaveChangesAsync();
    }
}
