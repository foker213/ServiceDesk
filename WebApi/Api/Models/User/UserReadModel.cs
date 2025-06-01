namespace Api.Models.User;

public class UserReadModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool Blocked { get; set; }
}
