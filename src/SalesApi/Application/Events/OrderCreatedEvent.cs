namespace SalesApi.Application.Events;

public record OrderCreatedEvent
{
    public int OrderId { get; init; }
    public int CustomerId { get; init; }
    public List<OrderItemEvent> Items { get; init; } = new();
}

public record OrderItemEvent(int ProductId, int Quantity);
