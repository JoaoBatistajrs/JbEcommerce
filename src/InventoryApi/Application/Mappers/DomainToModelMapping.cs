using AutoMapper;
using InventoryApi.Application.ApiModels;
using InventoryApi.Domain.Entities;

namespace InventoryApi.Application.Mappers;

public class DomainToModelMapping : Profile
{
    public DomainToModelMapping()
    {
        CreateMap<Product, ProductModelRequest>().ReverseMap();
        CreateMap<Product, ProductModelResponse>().ReverseMap();
    }
}