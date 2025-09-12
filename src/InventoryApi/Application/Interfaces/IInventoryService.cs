using InventoryApi.Domain.Entities;

namespace InventoryApi.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<Product?> GetProductByIdAsync(int id);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> CreateProductAsync(string name, string description, decimal price);
        Task<bool> DeleteProductAsync(int id);

        Task<int> GetStockAsync(int productId);
        Task<List<StockMovement>> GetMovementsAsync(int productId);

        Task<bool> RegisterEntryAsync(int productId, int quantity, string reason);
        Task<bool> RegisterExitAsync(int productId, int quantity, string reason);
    }
}
