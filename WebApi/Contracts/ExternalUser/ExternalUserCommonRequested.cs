namespace ServiceDesk.Contracts.ExternalUser;

public class ExternalUserCommonRequested
{
    public int UserId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public required string FullName { get; set; }
}
