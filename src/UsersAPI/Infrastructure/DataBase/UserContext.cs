using Microsoft.EntityFrameworkCore;
using UsersApi.Domain.Entities;

namespace UsersApi.Infrastructure.DataBase;

public class UserContext : DbContext
{

    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Name = "admin",
                Email = "admin@example.com",
                Password = "AQAAAAIAAYagAAAAEMbTHMi7MmP1era8Gvb1Ni7h2dzE/zt3z1GYWzaPzoc2qP8R7Schaec1Bh7Qfi0cIw==",
                Role = Domain.Enum.UserRole.Adm,
                CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        base.OnModelCreating(modelBuilder);
    }

}

