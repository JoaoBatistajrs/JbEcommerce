using SalesApi.Domain.Entities;

namespace SalesApi.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Order order, CancellationToken cancellationToken);
    Task UpdateAsync(Order order);
    Task<IEnumerable<Order>> GetAllAsync();
}
