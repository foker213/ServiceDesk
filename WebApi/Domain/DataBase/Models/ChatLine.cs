namespace ServiceDesk.Domain.Database.Models;

public class ChatLine : Entity
{
    /// <summary>
    /// ID чата
    /// </summary>
    public Chat? Chat { get; set; } = default!;
    public int ChatId { get; set; }

    /// <summary>
    /// Сообщение
    /// </summary>
    public required string Message { get; set; }
}
