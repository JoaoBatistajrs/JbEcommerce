namespace SalesApi.Application.Commands;

public record CreateOrderCommand(int CustomerId, List<OrderItemCommand> Items);

public record OrderItemCommand(int ProductId, int Quantity, decimal Price);
