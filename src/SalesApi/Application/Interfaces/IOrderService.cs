using SalesApi.Application.Commands;
using SalesApi.Application.DTOs;

namespace SalesApi.Application.Interfaces;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(CreateOrderCommand command, CancellationToken cancellationToken = default);
    Task<OrderDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

