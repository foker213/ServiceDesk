namespace Api.Models.Request;

public class RequestCreated
{
    public Guid ChatId { get; set; }
    public required string Description { get; set; }
}
