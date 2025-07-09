namespace ServiceDesk.Contracts.ExternalUser;

public class UserCheckRequested
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public required string FIO { get; set; }
}
