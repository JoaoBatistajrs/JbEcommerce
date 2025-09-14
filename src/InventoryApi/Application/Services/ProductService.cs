using AutoMapper;
using InventoryApi.Application.ApiModels;
using InventoryApi.Application.Interfaces;
using InventoryApi.Domain.Entities;
using InventoryApi.Domain.Intarfaces.Repository;

namespace InventoryApi.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductModelResponse> CreateProductAsync(ProductModelRequest request)
        {
            var product = _mapper.Map<Product>(request);

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return _mapper.Map<ProductModelResponse>(product);
        }

        public async Task<List<ProductModelResponse>> GetAll()
        {
            var products = await _productRepository.GetAllAsync();
            var response = _mapper.Map<List<ProductModelResponse>>(products);
            return response;
        }

        public Task<ProductModelResponse> GetById(int id)
        {
            var product = _productRepository.GetByIdAsync(id);
            return product.ContinueWith(t => _mapper.Map<ProductModelResponse>(t.Result));
        }
    }
}
