using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Console = GameRental.Domain.Entities.Console;

namespace GameRental.Infrastructure.Configurations;

public class ConsoleConfiguration:IEntityTypeConfiguration<Console>
{
    public void Configure(EntityTypeBuilder<Console> builder)
    {
        builder.ToTable(nameof(Console));
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Name).IsRequired();
        builder.HasMany(u => u.Accounts)
            .WithMany(u => u.Consoles);
        builder.HasData(
            new Console() { Id = 1,Name = "PS4" },
            new Console() { Id = 2,Name = "PS5" }
        );
    }
}