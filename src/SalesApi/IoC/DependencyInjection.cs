using Microsoft.EntityFrameworkCore;
using SalesApi.Application.Interfaces;
using SalesApi.Application.Services;
using SalesApi.Infrastructure.Database;
using SalesApi.Infrastructure.Messaging;
using SalesApi.Infrastructure.Repositories;

namespace SalesApi.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraStrucuture(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SalesContext>(options =>
                        options.UseNpgsql(
                            configuration.GetConnectionString("DefaultConnection"),
                            npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 5,   
                                maxRetryDelay: TimeSpan.FromSeconds(10),
                                errorCodesToAdd: null)
                            )
                        );

        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOrderService, OrderService>();

        services.AddSingleton<RabbitMqEventPublisher>();
        services.AddSingleton<IEventPublisher>(sp => sp.GetRequiredService<RabbitMqEventPublisher>());


        services.AddHostedService<RabbitMqHostedService>();

        return services;
    }
}
