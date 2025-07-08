using Domain.DataBase.Models;
using Infrastructure.DataBase;
using Infrastructure.Repository.IRepository;

namespace Infrastructure.Repository;

internal class ChatLineRepository(
    ServiceDeskDbContext db,
    TimeProvider tp
) : Repository<ChatLine>(db, tp), IRequestRepository
{

}
