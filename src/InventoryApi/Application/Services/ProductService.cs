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

        public async Task<ProductModelApi> CreateProductAsync(ProductModelApi request)
        {
            var product = _mapper.Map<Product>(request);

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return _mapper.Map<ProductModelApi>(product);
        } 
    }
}
