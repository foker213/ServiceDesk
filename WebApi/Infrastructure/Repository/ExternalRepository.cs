using Domain.DataBase.Models;
using Infrastructure.DataBase;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

internal sealed class ExternalRepository(
    ServiceDeskDbContext db,
    TimeProvider tp
) : Repository<ExternalUser>(db, tp), IExternalUserRepository
{
    public async Task<ExternalUser?> GetByEmail(string email)
    {
        var query = GetQuery();
        return await query.Where(x => x.Email == email).FirstOrDefaultAsync();
    }

    public async Task<ExternalUser?> GetByNumber(string number)
    {
        var query = GetQuery();
        return await query.Where(x => x.NumberPhone == number).FirstOrDefaultAsync();
    }
}
