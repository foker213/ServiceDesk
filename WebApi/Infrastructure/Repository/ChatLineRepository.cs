using ServiceDesk.Domain.Database.Models;
using ServiceDesk.Infrastructure.Database;
using ServiceDesk.Application.IRepository;

namespace ServiceDesk.Infrastructure.Repository;

internal sealed class ChatLineRepository(
    ServiceDeskDbContext db,
    TimeProvider tp
) : Repository<ChatLine>(db, tp), IChatLineRepository
{

}
