using Api.Models.Chat;
using MassTransit;

namespace Api.Consumers;

public class ChatInitiatedConsumer : IConsumer<ChatInitiated>
{
    public ChatInitiatedConsumer()
    {

    }

    public async Task Consume(ConsumeContext<ChatInitiated> context)
    {

    }
}