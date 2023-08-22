using GameRental.Domain.IRepository;

namespace GameRental.Application.Interfaces;

public interface IServiceManager
{
    IProductService ProductService { get; }
    IProductCategoryService ProductCategoryService { get; }
    IAccountService AccountService { get; }
    IConsoleService ConsoleService { get; }
    IIdentityService IdentityService { get; }
    IOrderService OrderService { get; }
}