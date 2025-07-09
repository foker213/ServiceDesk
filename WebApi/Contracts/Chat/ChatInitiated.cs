namespace ServiceDesk.Contracts.Chat;

public class ChatInitiated
{
    public Guid UserId { get; set; }
    public required string TelegramChatId { get; set; }
}
