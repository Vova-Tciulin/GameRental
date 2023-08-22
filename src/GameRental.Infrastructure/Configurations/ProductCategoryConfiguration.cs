using GameRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRental.Infrastructure.Configurations;

public class ProductCategoryConfiguration:IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable(nameof(ProductCategory));

        builder.HasKey(category => category.Id);

        builder.Property(category => category.Name)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(category => category.ImgPath).IsRequired();
        
        builder.HasMany(u => u.Products)
            .WithMany(u => u.ProductCategories);
    }
}