namespace ServiceDesk.TelegramBot.State;

public interface IUserStateService
{
    void SetUserState(long chatId, UserState state);
    UserState GetUserState(long chatId);
    void ClearUserState(long chatId);
    void SetUserData(long chatId, string key, string value);
    string GetUserData(long chatId, string key);
    void ClearUserData(long chatId);
}
