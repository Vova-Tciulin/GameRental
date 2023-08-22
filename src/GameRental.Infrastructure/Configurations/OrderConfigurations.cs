using GameRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRental.Infrastructure.Configurations;

public class OrderConfigurations:IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(nameof(Order));
        
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Cost)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        builder.Property(u => u.OrderDate)
            .HasColumnType("Date");
        builder.Property(u => u.EndOrderDate)
            .HasColumnType("Date");
        builder.Property(u => u.IsActive).IsRequired();
        builder.HasOne(u => u.Account)
            .WithMany(u => u.Orders)
            .HasForeignKey(u => u.AccountId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(u => u.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}