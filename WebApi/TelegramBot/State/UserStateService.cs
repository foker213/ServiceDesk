namespace ServiceDesk.TelegramBot.State;

public class UserStateService : IUserStateService
{
    private readonly Dictionary<long, UserState> _userStates = new();

    public UserState GetUserState(long chatId)
    {
        return _userStates.TryGetValue(chatId, out var state) ? state : UserState.None;
    }

    public void SetUserState(long chatId, UserState state)
    {
        _userStates[chatId] = state;
    }

    public void ClearUserState(long chatId)
    {
        _userStates.Remove(chatId);
    }
}
