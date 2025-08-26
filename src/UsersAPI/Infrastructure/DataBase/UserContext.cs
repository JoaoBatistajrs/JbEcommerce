using Microsoft.EntityFrameworkCore;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Infrastructure.DataBase;

public class UserContext : DbContext
{

    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

}

