using Microsoft.EntityFrameworkCore;
using SalesApi.Application.Interfaces;
using SalesApi.Infrastructure.Database;

namespace SalesApi.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraStrucuture(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SalesContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IOrderRepository, IOrderRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
