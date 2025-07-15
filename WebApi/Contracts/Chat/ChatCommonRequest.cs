namespace ServiceDesk.Contracts.Chat;

public class ChatCommonRequest
{
    public int ExternalUserId { get; set; }
    public required long TelegramChatId { get; set; }
}
