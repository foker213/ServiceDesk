namespace Api.Models.User;

public class UserReadModel
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool Blocked { get; set; }
    public string? BlockedReason { get; set; }
}
