using GameRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRental.Infrastructure.Configurations;

public class AccountConfiguration:IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable(nameof(Account));
        builder.HasKey(u => u.Id);
        builder.Property(u => u.ProductId).IsRequired(false);
        builder.Property(u => u.AccountNumber).IsRequired();
        builder.Property(u => u.Login).IsRequired();
        builder.Property(u => u.Password).IsRequired();
        builder.Property(u => u.IsRented).IsRequired();
        builder.Property(u=>u.Price).IsRequired()
            .HasColumnType("decimal(18,2)");
        builder.HasOne(u => u.Product)
            .WithMany(u => u.Accounts)
            .HasForeignKey(u=>u.ProductId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}