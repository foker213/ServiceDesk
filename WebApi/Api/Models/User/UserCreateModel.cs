using System.ComponentModel.DataAnnotations;

namespace Api.Models.User;

public class UserCreateModel
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string MiddleName { get; set; }

    public required string Login { get; set; }

    public required string Password { get; set; }

    [EmailAddress]
    public required string Email { get; set; }

}
