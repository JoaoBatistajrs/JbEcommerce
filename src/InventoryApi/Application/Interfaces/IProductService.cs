using InventoryApi.Application.ApiModels;

namespace InventoryApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductModelResponse> CreateProductAsync(ProductModelRequest request);
    }
}