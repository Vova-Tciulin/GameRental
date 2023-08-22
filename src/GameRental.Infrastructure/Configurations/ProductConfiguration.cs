using GameRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRental.Infrastructure.Configurations;

public class ProductConfiguration:IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(nameof(Product));

        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(u => u.Description).IsRequired();
        builder.Property(u => u.TransitTime).IsRequired();
        builder.Property(u => u.Translate).IsRequired();
        builder.Property(u => u.Year).IsRequired();
        
        builder.HasMany(u => u.ProductCategories)
            .WithMany(u => u.Products);
        builder.HasMany(u => u.Accounts)
            .WithOne(u => u.Product);
        builder.HasMany(u => u.Images)
            .WithOne(u => u.Product);
        
    }
}