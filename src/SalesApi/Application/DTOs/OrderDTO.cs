namespace SalesApi.Application.DTOs;

public record OrderItemDto(int ProductId, int Quantity, decimal Price);

public record OrderDto(
    int Id,
    int CustomerId,
    IReadOnlyCollection<OrderItemDto> Items,
    decimal Total,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
