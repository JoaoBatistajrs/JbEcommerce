using InventoryApi.Application.Interfaces;
using InventoryApi.Application.Mappers;
using InventoryApi.Application.Services;
using InventoryApi.Domain.Intarfaces.Repository;
using InventoryApi.Infrastructure.Configuration;
using InventoryApi.Infrastructure.Database;
using InventoryApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraStrucuture(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InventoryContext>(options =>
                        options.UseNpgsql(
                            configuration.GetConnectionString("DefaultConnection"),
                            npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(10),
                                errorCodesToAdd: null)
                            )
                        );

        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddAutoMapper(cfg => { }, typeof(DomainToModelMapping));
        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMQ"));

        return services;
    }
}