namespace Api.Models.Chat;

public class ChatInitiated
{
    public Guid UserId { get; set; }
    public required string TelegramChatId { get; set; }
}
