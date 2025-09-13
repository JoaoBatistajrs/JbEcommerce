using InventoryApi.Application.ApiModels;

namespace InventoryApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductModelApi> CreateProductAsync(ProductModelApi request);
    }
}