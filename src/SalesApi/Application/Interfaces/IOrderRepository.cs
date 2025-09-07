using SalesApi.Domain.Entities;

namespace SalesApi.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task<IEnumerable<Order>> GetAllAsync();
}
