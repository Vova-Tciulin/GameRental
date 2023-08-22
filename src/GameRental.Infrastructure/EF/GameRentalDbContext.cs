using GameRental.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Console = GameRental.Domain.Entities.Console;

namespace GameRental.Infrastructure.EF;

public class GameRentalDbContext:IdentityDbContext<User>
{
    private readonly string _connectionString="Server=(localdb)\\MSSQLLocalDB;Database=GameRentalDB;Trusted_Connection=True";

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Console> Consoles { get; set; }
    public DbSet<Order> Orders { get; set; }

    public GameRentalDbContext()
    {
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameRentalDbContext).Assembly);
        
    }
}