using System.ComponentModel.DataAnnotations;

namespace Api.Models.User;

public class UserChangeModel
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string MiddleName { get; set; }

    [EmailAddress]
    public required string Email { get; set; }

    public string? PhoneNumber { get; set; }

    public bool Blocked { get; set; }

    public string? BlockedReason { get; set; }

    public string Name
    {
        get
        {
            return $"{LastName.Trim()} {FirstName.Trim()} {MiddleName.Trim()}";
        }
    }

}
