namespace ServiceDesk.Contracts;

public class OperationResult<T>
{
    public T? Value { get; set; }
    public bool IsError { get; set; }
    public string? ErrorMessage { get; set; }
}