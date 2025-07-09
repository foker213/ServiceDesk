namespace ServiceDesk.Contracts;

public record PagingModel<TValue>(
    int Total,
    List<TValue> Data
);