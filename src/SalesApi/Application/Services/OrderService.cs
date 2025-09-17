using SalesApi.Application.Commands;
using SalesApi.Application.DTOs;
using SalesApi.Application.Events;
using SalesApi.Application.Interfaces;
using SalesApi.Domain.Entities;


namespace SalesApi.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEventPublisher _eventPublisher;

    public OrderService(IOrderRepository orderRepository, IEventPublisher eventPublisher)
    {
        _orderRepository = orderRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        var order = new Order(command.CustomerId);

        foreach (var item in command.Items)
        {
            order.AddItem(item.ProductId, item.Quantity, item.Price);
        }

        await _orderRepository.AddAsync(order, cancellationToken);

        await _eventPublisher.PublishAsync(
            new OrderCreatedEvent
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                Items = order.Items.Select(i => new OrderItemEvent(i.ProductId, i.Quantity)).ToList()
            },
            exchange: "order.exchange",
            cancellationToken: cancellationToken
        );

        return new OrderDto(
            order.Id,
            order.CustomerId,
            order.Items.Select(item => new OrderItemDto(item.ProductId, item.Quantity, item.Price)).ToList(),
            order.Total,
            order.Status.ToString(),
            order.CreatedAt,
            order.UpdatedAt
        );
    }

    public async Task<OrderDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);

        if (order is null) return null;

        return new OrderDto(
            order.Id,
            order.CustomerId,
            order.Items.Select(i => new OrderItemDto(i.ProductId, i.Quantity, i.Price)).ToList(),
            order.Total,
            order.Status.ToString(),
            order.CreatedAt,
            order.UpdatedAt
        );
    }
}

