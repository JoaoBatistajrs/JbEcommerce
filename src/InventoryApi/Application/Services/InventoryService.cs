using InventoryApi.Application.Interfaces;
using InventoryApi.Domain.Entities;
using InventoryApi.Domain.Enums;
using InventoryApi.Domain.Intarfaces.Repository;

namespace InventoryApi.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IProductRepository _productRepository;

        public InventoryService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product> CreateProductAsync(string name, string description, decimal price)
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            await Task.Run(() => _productRepository.DeleteAsync(product));
            await _productRepository.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetStockAsync(int productId)
        {
            return await _productRepository.GetStockAsync(productId);
        }

        public async Task<List<StockMovement>> GetMovementsAsync(int productId)
        {
            return await _productRepository.GetMovementsAsync(productId);
        }

        public async Task<bool> RegisterEntryAsync(int productId, int quantity, string reason)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return false;

            var movement = new StockMovement
            {
                Quantity = quantity,
                Direction = MovementDirection.Entry,
                Reason = reason
            };

            await _productRepository.AddMovementAsync(productId, movement);
            await _productRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RegisterExitAsync(int productId, int quantity, string reason)
        {
            var currentStock = await _productRepository.GetStockAsync(productId);
            if (currentStock < quantity) return false;

            var movement = new StockMovement
            {
                Quantity = quantity,
                Direction = MovementDirection.Exit,
                Reason = reason
            };

            await _productRepository.AddMovementAsync(productId, movement);
            await _productRepository.SaveChangesAsync();

            return true;
        }
    }
}
