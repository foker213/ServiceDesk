namespace ServiceDesk.Contracts.ExternalUser;

public class ExternalUserCommonRequest
{
    public int UserId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public required string FullName { get; set; }
}
