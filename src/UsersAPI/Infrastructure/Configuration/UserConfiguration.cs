using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsersApi.Domain.Entities;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .ValueGeneratedOnAdd();

        builder.Property(u => u.Name)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(u => u.Email)
               .HasMaxLength(50);

        builder.Property(u => u.Password)
               .HasMaxLength(255);

        builder.Property(u => u.Role)
               .IsRequired();

        builder.Property(u => u.CreatedAt)
               .IsRequired()
               .HasDefaultValueSql("NOW()"); 

        builder.Property(u => u.UpdatedAt)
               .IsRequired()
               .HasDefaultValueSql("NOW()"); 

        builder.HasIndex(u => u.Email)
               .IsUnique(false);
    }
}
