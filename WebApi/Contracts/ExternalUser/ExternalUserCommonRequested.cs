namespace ServiceDesk.Contracts.ExternalUser;

public class ExternalUserCommonRequested
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public required string FullName { get; set; }
}
