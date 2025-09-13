using AutoMapper;
using InventoryApi.Application.ApiModels;
using InventoryApi.Domain.Entities;

namespace InventoryApi.Application.Mappers;

public class ModelToDomainMapping : Profile
{
    public ModelToDomainMapping()
    {
        CreateMap<ProductModelRequest, Product>();
        CreateMap<ProductModelResponse, Product>();
    }
}
