using InventoryApi.Domain.Entities;

namespace InventoryApi.Domain.Intarfaces.Repository;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<List<Product>> GetAllAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);

    Task<int> GetStockAsync(int productId);
    Task<List<StockMovement>> GetMovementsAsync(int productId);
    Task AddMovementAsync(int productId, StockMovement movement);

    Task SaveChangesAsync();
}
