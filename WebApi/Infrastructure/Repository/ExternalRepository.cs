using Microsoft.EntityFrameworkCore;
using ServiceDesk.Application.IRepository;
using ServiceDesk.Domain.Database.Models;
using ServiceDesk.Infrastructure.Database;

namespace ServiceDesk.Infrastructure.Repository;

internal sealed class ExternalRepository(
    ServiceDeskDbContext db,
    TimeProvider tp
) : Repository<ExternalUser>(db, tp), IExternalUserRepository
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
}
