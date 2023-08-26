using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GameRental.Application.DTO.Order;
using GameRental.Application.Exceptions;
using GameRental.Application.Services;
using GameRental.Domain.Entities;
using GameRental.Domain.Exceptions;
using GameRental.Domain.IRepository;
using GameRental.Infrastructure.Email.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework.Internal;

namespace ApplicationTests.ServicesUnitTests;

[TestFixture]
public class OrderServiceTests
{
    private  Mock<IUnitOfWork> _dbMock;
    private  Mock<IMapper> _mapMock;
    private  Mock<IValidator<OrderAddDto>> _orderForAddValidatorMock;
    private  Mock<UserManager<User>> _userManagerMock;
    private  Mock<IEmailSender> _emailSenderMock;
    private  Mock<IOrderRepository> _orderRepositoryMock;
    private  OrderService _orderService;
    
    [SetUp]
    public void SetUp()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _mapMock = new Mock<IMapper>();
        _orderForAddValidatorMock = new Mock<IValidator<OrderAddDto>>();
        _userManagerMock = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object);
        _emailSenderMock = new Mock<IEmailSender>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _orderService = new OrderService(
            _dbMock.Object, _mapMock.Object, _orderForAddValidatorMock.Object,
            _userManagerMock.Object, _emailSenderMock.Object);

        _dbMock.Setup(u => u.OrderRepository).Returns(_orderRepositoryMock.Object);
    }

    [Test]
    public async Task GetOrderByIdAsync_OrderExist_ReturnOrderDto()
    {
        //arrange
        var orderId = 1;
        var order = new Order() { Id = 1 };
        var orderDto = new OrderDto() { Id = 1 };
        _mapMock.Setup(u => u.Map<OrderDto>(order)).Returns(orderDto);
        _dbMock.Setup(u => u.OrderRepository.GetByIdAsync(orderId, false)).ReturnsAsync(order);
        
        //act
        var result = await _orderService.GetOrderByIdAsync(orderId);
        
        //assert
        _dbMock.Verify(u=>u.OrderRepository.GetByIdAsync(orderId,false),Times.Once);
        _mapMock.Verify(u=>u.Map<OrderDto>(order),Times.Once);
        Assert.That(result.Id,Is.EqualTo(orderId));
        
    }
    
    [Test]
    public void GetOrderByIdAsync_OrderNotExist_ReturnOrderNotFoundException()
    {
        //arrange
        var orderId = 1;
        _dbMock.Setup(u => u.OrderRepository.GetByIdAsync(orderId, false)).ReturnsAsync((Order)null);
        
        //act && assert
        Assert.ThrowsAsync<OrderNotFoundException>(() => _orderService.GetOrderByIdAsync(orderId));
    }

    [Test]
    public async Task GetOrdersAsync_ReturnsOrdersDto()
    {
        //arrange
        var orders = new List<Order>() { new Order(), new Order(), new Order() };
        var ordersDto = new List<OrderDto>() { new OrderDto(), new OrderDto(), new OrderDto() };
        _mapMock.Setup(u => u.Map<List<OrderDto>>(orders)).Returns(ordersDto);
        _dbMock.Setup(u => u.OrderRepository.GetOrdersAsync(null)).ReturnsAsync(orders);
        
        //act
        var result = await _orderService.GetOrdersAsync();
        
        //assert
        Assert.That(result,Is.EqualTo(ordersDto));
    }

    [Test]
    public async Task CreateOrdersync_ValidOrder_CreateOrder()
    {
        //arrange
        var orderAddDto = new OrderAddDto() { AccountId = 1, Cost = 100, DayOfRent = 7, UserEmail = "testEmail" };
        var order = new Order() { AccountId = 1, Cost = 100 };
        var user = new User() { Email = "testEmail" };
        var validationResult = new ValidationResult();
        var account = new Account() { Id = 1 };
        _orderForAddValidatorMock
            .Setup(v => v.ValidateAsync(orderAddDto,default))
            .ReturnsAsync(validationResult);
        _dbMock.Setup(u => u.AccountRepository.GetByIdAsync(orderAddDto.AccountId, true)).ReturnsAsync(account);
        _mapMock.Setup(u => u.Map<Order>(orderAddDto)).Returns(order);
        _userManagerMock.Setup(u => u.FindByEmailAsync(orderAddDto.UserEmail)).ReturnsAsync(user);
        
        //act
        await _orderService.CreateOrderAsync(orderAddDto);
        
        //assert
        Assert.That(order.IsActive,Is.EqualTo(true));
        Assert.That(account.IsRented,Is.EqualTo(true));
        Assert.That(order.OrderDate,Is.EqualTo(DateTime.Today));
        _dbMock.Verify(u=>u.OrderRepository.Add(order),Times.Once);
        _dbMock.Verify(u=>u.SaveChangesAsync(),Times.Once);
        
    }

    [Test]
    public void CreateOrderAsync_InvalidOrderDto_ReturnNotValidateModelException()
    {
        var orderDto = new OrderAddDto();
        var validationFailure = new ValidationFailure("test", "Error message test");
        var validationResult = new ValidationResult(new []{validationFailure});
        _orderForAddValidatorMock
            .Setup(u => u.ValidateAsync(orderDto,default))
            .ReturnsAsync(validationResult);
        
        //act && assert
        Assert.ThrowsAsync<NotValidateModelException>(() => _orderService.CreateOrderAsync(orderDto));
    }

    [Test]
    public void CreateOrderAsync_AccountIsNull_ReturnAccountNotFoundException()
    {
        var orderDto = new OrderAddDto() { AccountId = 1 };
        var validationResult = new ValidationResult();
        _orderForAddValidatorMock
            .Setup(v => v.ValidateAsync(orderDto,default))
            .ReturnsAsync(validationResult);
        _dbMock.Setup(u => u.AccountRepository.GetByIdAsync(orderDto.AccountId, true)).ReturnsAsync((Account)null);
        
        //act && assert
        Assert.ThrowsAsync<AccountNotFoundException>(() => _orderService.CreateOrderAsync(orderDto));
    }
    
    [Test]
    public void CreateOrderAsync_UserIsNull_ReturnUserNotFoundException()
    {
        var orderDto = new OrderAddDto() { AccountId = 1,UserEmail = "TestEmail"};
        var account = new Account() { Id = 1 };
        var validationResult = new ValidationResult();
        _orderForAddValidatorMock
            .Setup(v => v.ValidateAsync(orderDto,default))
            .ReturnsAsync(validationResult);
        _dbMock.Setup(u => u.AccountRepository.GetByIdAsync(orderDto.AccountId, true)).ReturnsAsync(account);
        _userManagerMock.Setup(u => u.FindByEmailAsync(orderDto.UserEmail)).ReturnsAsync((User)null);
        
        //act && assert
        Assert.ThrowsAsync<UserNotFoundException>(() => _orderService.CreateOrderAsync(orderDto));
    }
    
}