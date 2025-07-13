namespace ServiceDesk.Contracts.Chat;

public class ChatInitiated
{
    public int ExternalUserId { get; set; }
    public required long TelegramChatId { get; set; }
}
