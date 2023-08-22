using GameRental.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GameRental.Domain.IRepository;

public interface IUnitOfWork
{
    IAccountRepository AccountRepository { get; }
    IConsoleRepository ConsoleRepository { get; }
    IProductRepository ProductRepository { get; }
    IProductCategoryRepository ProductCategoryRepository { get; }
    IImageRepository ImageRepository { get; }
    IOrderRepository OrderRepository { get; }
    Task SaveChangesAsync();
}