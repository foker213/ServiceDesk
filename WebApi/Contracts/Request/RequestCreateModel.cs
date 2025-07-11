using ServiceDesk.Contracts.ExternalUser;

namespace ServiceDesk.Contracts.Request;

public class RequestCreateModel
{
    public long ChatId { get; set; }
    public required string Description { get; set; }
    public required ExternalUserCommonRequested ExternalUser { get; set; }
}
