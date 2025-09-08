using Microsoft.EntityFrameworkCore;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Infrastructure.DataBase;

public class UserContext : DbContext
{

    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserContext).Assembly);

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Name = "admin",
                Email = "admin@example.com",
                Password = "AQAAAAIAAYagAAAAEMbTHMi7MmP1era8Gvb1Ni7h2dzE/zt3z1GYWzaPzoc2qP8R7Schaec1Bh7Qfi0cIw==",
                Role = Domain.Enum.UserRole.Adm
            }
        );

        base.OnModelCreating(modelBuilder);
    }

}

