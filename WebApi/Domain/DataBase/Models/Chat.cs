namespace ServiceDesk.Domain.Database.Models;

public class Chat : Entity
{
    /// <summary>
    /// Id пользователя из Telegram
    /// </summary>
    public ExternalUser? ExternalUser { get; set; } = default!;
    public int ExternalUserId { get; set; }

    /// <summary>
    /// ID телеграмм чата 
    /// </summary>
    public long TelegramChatId { get; set; }

    /// <summary>
    /// Прекрепленная заявка
    /// </summary>
    public Request? Request { get; set; }
}
