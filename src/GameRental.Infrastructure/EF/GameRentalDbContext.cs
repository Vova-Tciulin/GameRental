using GameRental.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Console = GameRental.Domain.Entities.Console;

namespace GameRental.Infrastructure.EF;

public class GameRentalDbContext:IdentityDbContext<User>
{
    
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductCategory> ProductCategories { get; set; }
    public virtual DbSet<Image> Images { get; set; }
    public  virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Console> Consoles { get; set; }
    public virtual DbSet<Order> Orders { get; set; }

    public GameRentalDbContext(DbContextOptions<GameRentalDbContext> options)
        :base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameRentalDbContext).Assembly);
        
    }
}