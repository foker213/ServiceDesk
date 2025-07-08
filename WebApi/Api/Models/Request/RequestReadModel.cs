using Domain.DataBase.Enums;

namespace Api.Models.Request;

public class RequestReadModel
{
    public int RequestId { get; set; }
    public int UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public Status Status { get; set; }
    public int ChatId { get; set; }
    public DateTime? DateStartRequest { get; set; }
    public DateTime? DateEndRequest { get; set; }
    public DateTime? CreateAt { get; set; }
}
