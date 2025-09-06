using Microsoft.EntityFrameworkCore;
using UsersAPI.Application.Services;
using UsersAPI.Domain.Interfaces.Repository;
using UsersAPI.Domain.Interfaces.Service;
using UsersAPI.Infrastructure.DataBase;
using UsersAPI.Infrastructure.Repository;

namespace UsersAPI.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraStrucuture(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordService, PasswordService>();

        return services;
    }
}
