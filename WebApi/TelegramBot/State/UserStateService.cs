namespace ServiceDesk.TelegramBot.State;

public class UserStateService : IUserStateService
{
    private readonly Dictionary<long, UserState> _userStates = new();
    private readonly Dictionary<long, Dictionary<string, string>> _userData = new();

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

    public void SetUserData(long chatId, string key, string value)
    {
        if (!_userData.ContainsKey(chatId))
            _userData[chatId] = new Dictionary<string, string>();
        
        _userData[chatId][key] = value;
    }
    
    public string GetUserData(long chatId, string key)
    {
        if (_userData.TryGetValue(chatId, out Dictionary<string, string>? data) && data.TryGetValue(key, out string? value))
            return value;
        
        return string.Empty; 
    }
    
    public void ClearUserData(long chatId) => _userData.Remove(chatId);
}