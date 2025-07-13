using ServiceDesk.Domain.Database.Models;
using ServiceDesk.Infrastructure.Database;
using ServiceDesk.Application.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ServiceDesk.Infrastructure.Repository;

internal sealed class ChatRepository(
    ServiceDeskDbContext db,
    TimeProvider tp
) : Repository<Chat>(db, tp), IChatRepository
{
    public async Task<int> GetId(long telegramChatId, bool noTracking = false)
    {
        var query = GetQuery(noTracking: noTracking);
        var result = await query.Where(x => x.TelegramChatId == telegramChatId).FirstOrDefaultAsync();

        return result?.Id ?? throw new Exception($"Объект с TelegramChatId {telegramChatId} не найден");
    }
}