namespace ServiceDesk.TelegramBot.Commands.InputHandlers.IInputHandler;

public interface IInputDataHandler
{
    Task HandleInputAsync(long chatId, string inputData, CancellationToken cancellationToken);
}
