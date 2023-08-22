using GameRental.Domain.Entities;
using GameRental.Domain.IRepository;
using GameRental.Infrastructure.EF;
using Microsoft.AspNetCore.Identity;

namespace GameRental.Infrastructure.Repository;

public class UnitOfWork:IUnitOfWork
{
    private readonly GameRentalDbContext _db;
   
    public IAccountRepository AccountRepository { get; }
    public IConsoleRepository ConsoleRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IProductCategoryRepository ProductCategoryRepository { get; }
    public IImageRepository ImageRepository { get; }
    public IOrderRepository OrderRepository { get; }
    
    public UnitOfWork(
        GameRentalDbContext db, IProductRepository productRepository,
        IProductCategoryRepository productCategoryRepository, IImageRepository imageRepository,
        IAccountRepository accountRepository, IConsoleRepository consoleRepository, IOrderRepository orderRepository)
    {
        _db = db;
        ProductRepository = productRepository;
        ProductCategoryRepository = productCategoryRepository;
        ImageRepository = imageRepository;
        AccountRepository = accountRepository;
        ConsoleRepository = consoleRepository;
        OrderRepository = orderRepository;
    }
    
    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}