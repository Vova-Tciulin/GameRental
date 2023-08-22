using GameRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRental.Infrastructure.Configurations;

public class ImageConfiguration: IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable(nameof(Image));

        builder.HasKey(image => image.Id);
        builder.Property(image => image.ImgName).IsRequired();
        builder.HasOne(u => u.Product)
            .WithMany(u => u.Images)
            .HasForeignKey(u => u.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}