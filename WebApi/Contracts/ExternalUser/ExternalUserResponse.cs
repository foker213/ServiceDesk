namespace ServiceDesk.Contracts.ExternalUser;

public class ExternalUserResponse
{
    public int UserId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public required string FullName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
