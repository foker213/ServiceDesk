namespace ServiceDesk.TelegramBot.State;

public enum UserState
{
    None,
    WaitingForFullName,
    WaitingForEmail,
    WaitingForPhone,
    WaitengForDescription
}
