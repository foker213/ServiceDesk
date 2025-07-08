using Api.Models.Request;
using MassTransit;

namespace Api.Consumers;

public class RequestCreatedConsumer : IConsumer<RequestCreated>
{
    public RequestCreatedConsumer()
    {

    }

    public async Task Consume(ConsumeContext<RequestCreated> context)
    {

    }
}