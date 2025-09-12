using InventoryApi.Domain.Entities;
using InventoryApi.Domain.Enums;
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

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Movements)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Movements)
                .ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<int> GetStockAsync(int productId)
        {
            return await _context.StockMovements
                .Where(m => m.ProductId == productId)
                .SumAsync(m => m.Direction == MovementDirection.Entry ? m.Quantity : -m.Quantity);
        }

        public async Task<List<StockMovement>> GetMovementsAsync(int productId)
        {
            return await _context.StockMovements
                .Where(m => m.ProductId == productId)
                .OrderByDescending(m => m.Date)
                .ToListAsync();
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
