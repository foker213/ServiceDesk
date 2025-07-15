using ServiceDesk.Contracts.ExternalUser;

namespace ServiceDesk.Contracts.Request;

public class RequestCommonRequest
{
    public long? ChatId { get; set; }
    public required string Description { get; set; }
}
