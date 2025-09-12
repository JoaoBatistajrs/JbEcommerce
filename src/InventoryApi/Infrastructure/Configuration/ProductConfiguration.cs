using InventoryApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryApi.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(p => p.Description)
               .HasMaxLength(1000);

        builder.Property(p => p.Price)
               .HasColumnType("decimal(18,2)");

        // Relacionamento 1:N (Product -> Movements)
        builder.HasMany(p => p.Movements)
               .WithOne(m => m.Product)
               .HasForeignKey(m => m.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
