namespace ServiceDesk.Contracts.Request;

public class RequestReadModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Status { get; set; } = default!;
    public int ChatId { get; set; }
    public DateTime? DateStartRequest { get; set; }
    public DateTime? DateEndRequest { get; set; }
    public required string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
