namespace ServiceDesk.TelegramBot.State;

public interface IUserStateService
{
    void SetUserState(long chatId, UserState state);
    UserState GetUserState(long chatId);
    void ClearUserState(long chatId);
}
