using AutoMapper;
using FluentValidation;
using GameRental.Application.DTO;
using GameRental.Application.DTO.Account;
using GameRental.Application.DTO.Category;
using GameRental.Application.DTO.Order;
using GameRental.Application.DTO.Product;
using GameRental.Application.Interfaces;
using GameRental.Domain.Entities;
using GameRental.Domain.IRepository;
using GameRental.Infrastructure.EF;
using GameRental.Infrastructure.Email.Intefaces;
using Microsoft.AspNetCore.Identity;

namespace GameRental.Application.Services;

public class ServiceManager:IServiceManager
{
    private readonly Lazy<IProductService> _productService;
    private readonly Lazy<IProductCategoryService> _productCategoryService;
    private readonly Lazy<IAccountService> _accountService;
    private readonly Lazy<IConsoleService> _consoleService;
    private readonly Lazy<IIdentityService> _identityService;
    private readonly Lazy<IOrderService> _orderService;

    public ServiceManager(IUnitOfWork db,
        IMapper map,
        IValidator<ProductForUpdateDto> productForUpdateDtoValidator,
        IValidator<ProductAddDto> productAddDtoValidator,
        IValidator<AccountForAddDto> accountForAddValidator,
        IValidator<AccountForUpdateDto> accountForUpdateDtoValidator,
        UserManager<User> userManager, SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager,IValidator<OrderAddDto>orderForAddValidator,
        IEmailSender emailSender,IImageOperations imageOperations)
    {
        _productService = new Lazy<IProductService>(() => new ProductService(db,map,productForUpdateDtoValidator,productAddDtoValidator,imageOperations));
        _productCategoryService = new Lazy<IProductCategoryService>(() => new ProductCategoryService(db,map,imageOperations));
        _accountService = new Lazy<IAccountService>(() => new AccountService(db, map,accountForAddValidator,accountForUpdateDtoValidator));
        _consoleService = new Lazy<IConsoleService>(() => new ConsoleService(db, map));
        _identityService = new Lazy<IIdentityService>(() => new IdentityService(map, userManager, signInManager,roleManager));
        _orderService = new Lazy<IOrderService>(() => new OrderService(db, map, orderForAddValidator,userManager,emailSender));
    }
    
    public IProductService ProductService => _productService.Value;
    public IProductCategoryService ProductCategoryService => _productCategoryService.Value;
    public IAccountService AccountService => _accountService.Value;
    public IConsoleService ConsoleService => _consoleService.Value;
    public IIdentityService IdentityService => _identityService.Value;
    public IOrderService OrderService => _orderService.Value;
}