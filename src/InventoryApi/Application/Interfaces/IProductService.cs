using InventoryApi.Application.ApiModels;

namespace InventoryApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductModelResponse> CreateProductAsync(ProductModelRequest request);
        Task<ProductModelResponse> GetById(int id);
        Task<List<ProductModelResponse>> GetAll();
    }
}