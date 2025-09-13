using InventoryApi.Domain.Entities;

namespace InventoryApi.Domain.Intarfaces.Repository;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<List<Product?>> GetByIdsAsync(List<int> ids);
    Task UpdateAsync(Product product);
    Task AddMovementAsync(int productId, StockMovement movement);
    Task SaveChangesAsync();
}