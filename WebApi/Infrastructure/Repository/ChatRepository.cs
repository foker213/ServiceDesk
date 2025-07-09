using ServiceDesk.Domain.Database.Models;
using ServiceDesk.Infrastructure.Database;
using ServiceDesk.Application.IRepository;

namespace ServiceDesk.Infrastructure.Repository;

internal sealed class ChatRepository(
    ServiceDeskDbContext db,
    TimeProvider tp
) : Repository<Chat>(db, tp), IChatRepository
{

}