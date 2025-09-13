using InventoryApi.Domain.Entities;
using InventoryApi.Domain.Intarfaces.Repository;
using InventoryApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly InventoryContext _context;

        public ProductRepository(InventoryContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task<List<Product>> GetByIdsAsync(List<int> ids)
        {
            return await _context.Products
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();
        }

        public Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return Task.CompletedTask;
        }

        public async Task AddMovementAsync(int productId, StockMovement movement)
        {
            movement.ProductId = productId;
            await _context.StockMovements.AddAsync(movement);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}