using Api.Models.ExternalUser;
using MassTransit;

namespace Api.Consumers;

public class UserCheckConsumer : IConsumer<UserCheckRequested>
{
    public UserCheckConsumer()
    {

    }

    public async Task Consume(ConsumeContext<UserCheckRequested> context)
    {

    }
}