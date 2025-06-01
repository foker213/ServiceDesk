namespace Api.Models;

public record PagingModel<TValue>(
    int Total,
    List<TValue> Data
);