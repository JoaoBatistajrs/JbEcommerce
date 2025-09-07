using Microsoft.EntityFrameworkCore;
using SalesApi.Application.Interfaces;
using SalesApi.Domain.Entities;
using SalesApi.Infrastructure.Database;

namespace SalesApi.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly SalesContext _context;

    public OrderRepository(SalesContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ToListAsync();
    }
}
