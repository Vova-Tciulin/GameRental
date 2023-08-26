using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GameRental.Application.DTO.Account;
using GameRental.Application.Exceptions;
using GameRental.Application.Interfaces;
using GameRental.Application.Services;
using GameRental.Domain.Entities;
using GameRental.Domain.Exceptions;
using GameRental.Domain.IRepository;
using Moq;
using Shouldly;

namespace ApplicationTests.ServicesUnitTests;
using Console = GameRental.Domain.Entities.Console;

public class AccountServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private Mock<IMapper> _mapperMock = null!;
    private Mock<IValidator<AccountForAddDto>> _accountForAddDtoValidatorMock=null!;
    private Mock<IValidator<AccountForUpdateDto>> _accountForUpdateDtoValidatorMock=null!;
    private Mock<IAccountRepository> _accountRepository = null!;
    private Mock<IProductRepository> _productRepository = null!;
    private Mock<IConsoleRepository> _consoleRepository = null!;
    private Mock<IOrderRepository> _orderRepository = null!;
    private AccountService _accountService=null!;
    
    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _accountRepository= new Mock<IAccountRepository>();
        _productRepository= new Mock<IProductRepository>();
        _consoleRepository= new Mock<IConsoleRepository>();
        _orderRepository = new Mock<IOrderRepository>();
        _accountForAddDtoValidatorMock = new Mock<IValidator<AccountForAddDto>>();
        _accountForUpdateDtoValidatorMock = new Mock<IValidator<AccountForUpdateDto>>();
        
        _accountService = new AccountService(
            _unitOfWorkMock.Object,_mapperMock.Object,
            _accountForAddDtoValidatorMock.Object,_accountForUpdateDtoValidatorMock.Object);

        _unitOfWorkMock.Setup(u => u.AccountRepository).Returns(_accountRepository.Object);
        _unitOfWorkMock.Setup(u => u.ProductRepository).Returns(_productRepository.Object);
        _unitOfWorkMock.Setup(u => u.ConsoleRepository).Returns(_consoleRepository.Object);
        _unitOfWorkMock.Setup(u => u.OrderRepository).Returns(_orderRepository.Object);
    }
    
    [Test]
    public void GetByIdAsync_AccountDoesntExist_ReturnAccountNotFoundException()
    {
        //arrange
        var accountId = 1;
        _unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(accountId,false,null)).ReturnsAsync((Account)null);
        
        //act & assert
        Assert.ThrowsAsync<AccountNotFoundException>(() => _accountService.GetByIdAsync(accountId));
    }
    
    [Test]
    public async Task GetByIdAsync_AccountExist_ReturnAccountDto()
    {
        // Arrange
        var accountId = 1;
        var accountEntity = new Account { Id = accountId, Name = "TestAccount" };
        var accountDto = new AccountDto() { Id = accountId, Name = "TestAccount" };
        
        _unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(accountId, false, It.IsAny<Expression<Func<Account, object>>[]>())).ReturnsAsync(accountEntity);
        _mapperMock.Setup(m => m.Map<AccountDto>(accountEntity)).Returns(accountDto);

        // Act
        var result = await _accountService.GetByIdAsync(accountId);

        // Assert
        Assert.That(result, Is.EqualTo(accountDto));
    }
    
    [Test]
    public async Task GetAllAsync_ReturnAccountsDto()
    {
        // Arrange
        List<Account> accounts = new List<Account>() { new Account(), new Account(), new Account() };
        List<AccountDto> accountsDto = new List<AccountDto>() { new AccountDto(), new AccountDto(), new AccountDto() };
        _unitOfWorkMock.Setup(u => u.AccountRepository.GetAccountsAsync( It.IsAny<Expression<Func<Account,bool>>>(),It.IsAny<Expression<Func<Account, object>>[]>())).ReturnsAsync(accounts);
        _mapperMock.Setup(m => m.Map<List<AccountDto>>(accounts)).Returns(accountsDto);

        // Act
        var result = await _accountService.GetAllAsync();

        // Assert
        Assert.That(result, Is.EqualTo(accountsDto));
    }

    [Test]
    public async Task AddAccountAsync_ValidAccountAndProductNotNull_AddAccount()
    {
        //arrange
        var accountDto = new AccountForAddDto(){SelectedConsoles = new []{1,2},ProductId = 1};
        var account = new Account();
        var product = new Product(){Id = 1,Name = "testName"};
        var accountNumber = 1;
        List<Console> consoles = new List<Console>() { new Console() { Id = 1 }, new Console() { Id = 2 } };
        var validationResult = new ValidationResult();
        _unitOfWorkMock
            .Setup(u => u.ProductRepository.GetByIdAsync(accountDto.ProductId, false))
            .ReturnsAsync(product);
        _unitOfWorkMock
            .Setup(u => u.AccountRepository.GetAccountsAsync(It.IsAny<Expression<Func<Account,bool>>>()))
            .ReturnsAsync(new List<Account>());
        _unitOfWorkMock
            .Setup(u => u.ConsoleRepository.GetConsolesAsync(u => accountDto.SelectedConsoles.Contains(u.Id)))
            .ReturnsAsync(consoles);
        _accountForAddDtoValidatorMock
            .Setup(v => v.ValidateAsync(accountDto,default))
            .ReturnsAsync(validationResult);
        _mapperMock.Setup(u => u.Map<Account>(accountDto)).Returns(account);
        
        //act
        await _accountService.AddAccountAsync(accountDto);
        
        //assert
        Assert.That(account.Name,Is.EqualTo(product.Name));
        Assert.That(account.AccountNumber,Is.EqualTo(accountNumber));
        Assert.That(account.Consoles,Is.EqualTo(consoles));
        _accountRepository.Verify(u=>u.Add(account),Times.Once);
        _unitOfWorkMock.Verify(u=>u.SaveChangesAsync(),Times.Once);
    }

    [Test]
    public void AddAccountAsync_InvalidAccount_ReturnNotValidateModelException()
    {
        //arrange
        var accountDto = new AccountForAddDto();
        var validationFailure = new ValidationFailure("test", "Error message test");
        var validationResult = new ValidationResult(new []{validationFailure});
        _accountForAddDtoValidatorMock
            .Setup(u => u.ValidateAsync(accountDto,default))
            .ReturnsAsync(validationResult);
        
        //act && assert
        Assert.ThrowsAsync<NotValidateModelException>(() => _accountService.AddAccountAsync(accountDto));

    }
    
    [Test]
    public void AddAccountAsync_ProductNotFound_ReturnProductNotFoundException()
    {
        //arrange
        var accountDto = new AccountForAddDto();
        var validationResult = new ValidationResult();
        _unitOfWorkMock.Setup(u => u.ProductRepository.GetByIdAsync(1, false)).ReturnsAsync((Product)null);
        _accountForAddDtoValidatorMock
            .Setup(v => v.ValidateAsync(accountDto,default))
            .ReturnsAsync(validationResult);
        
        //act && assert
        Assert.ThrowsAsync<ProductNotFoundExeption>(() => _accountService.AddAccountAsync(accountDto));
    }

    [Test]
    public async Task RemoveAsync_ValidId_RemoveAccount()
    {
        //arrange
        var id = 1;
        var account = new Account() { Id = 1 };
        _unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(id, false)).ReturnsAsync(account);
        
        //act
        await _accountService.RemoveAsync(id);
        
        //assert
        _unitOfWorkMock.Verify(u=>u.AccountRepository.Remove(account),Times.Once);
        _unitOfWorkMock.Verify(u=>u.SaveChangesAsync(),Times.Once);
    }
    
    [Test]
    public void RemoveAsync_AccountNotFound_ReturnAccountNotFoundException()
    {
        //arrange
        var id = 1;
        _unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(id, false)).ReturnsAsync((Account)null);
        
        //act && assert
        Assert.ThrowsAsync<AccountNotFoundException>(() => _accountService.RemoveAsync(id));
        
    }

    [Test]
    public void UpdateAsync_InvalidAccountForUpdate_ReturnNotValidateModelException()
    {
        //arrange
        var accountDto = new AccountForUpdateDto();
        var validationFailure = new ValidationFailure("test", "Error message test");
        var validationResult = new ValidationResult(new []{validationFailure});
        _accountForUpdateDtoValidatorMock
            .Setup(u => u.ValidateAsync(accountDto,default))
            .ReturnsAsync(validationResult);
        
        //act && assert
        Assert.ThrowsAsync<NotValidateModelException>(() => _accountService.UpdateAsync(accountDto));
    }
    
    [Test]
    public void UpdateAsync_AccountIsNotFound_ReturnAccountNotFoundException()
    {
        //arrange
        var accountDto = new AccountForUpdateDto(){Id = 1};
        var validationResult = new ValidationResult();
        _accountForUpdateDtoValidatorMock
            .Setup(u => u.ValidateAsync(accountDto,default))
            .ReturnsAsync(validationResult);
        _unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(accountDto.Id, false)).ReturnsAsync((Account)null);
        
        //act && assert
        Assert.ThrowsAsync<AccountNotFoundException>(() => _accountService.UpdateAsync(accountDto));
    }
    
    [Test]
    public void UpdateAsync_ProductIsNotFound_ReturnProductNotFoundException()
    {
        //arrange
        var accountDto = new AccountForUpdateDto(){Id = 1,ProductId = 1};
        var account = new Account() { Id = 1, ProductId = 2 };
        var validationResult = new ValidationResult();
        _accountForUpdateDtoValidatorMock
            .Setup(u => u.ValidateAsync(accountDto,default))
            .ReturnsAsync(validationResult);
        _unitOfWorkMock
            .Setup(u => u.AccountRepository
                .GetByIdAsync(accountDto.Id, true,It.IsAny<Expression<Func<Account,object>>>()))
            .ReturnsAsync(account);
        _unitOfWorkMock.Setup(u => u.ProductRepository.GetByIdAsync(accountDto.ProductId, false));
        
        //act && assert
        Assert.ThrowsAsync<ProductNotFoundExeption>(() => _accountService.UpdateAsync(accountDto));
    }
    
    [Test]
    public async Task UpdateAsync_AccountIsRentedEqualTrue_UpdateAccount()
    {
        //arrange
        var accountDto = new AccountForUpdateDto()
            {   Id = 1, 
                ProductId = 1, 
                IsRented = true,
                Price = 100,
                Login ="TestLogin",
                Password = "TestPassword",
                SelectedConsoles = new []{1,2}
            };
        List<Console> consoles = new List<Console>() { new Console() { Id = 1 }, new Console() { Id = 2 } };
        var account = new Account() { Id = 1, ProductId = 1 ,AccountNumber = 1, Name = "test",IsRented = true};
        var validationResult = new ValidationResult();
        
        _accountForUpdateDtoValidatorMock
            .Setup(u => u.ValidateAsync(accountDto,default))
            .ReturnsAsync(validationResult);
        _unitOfWorkMock
            .Setup(u => u.AccountRepository
                .GetByIdAsync(accountDto.Id, true,It.IsAny<Expression<Func<Account,object>>>()))
            .ReturnsAsync(account);
        _unitOfWorkMock
            .Setup(u => u.ConsoleRepository.GetConsolesAsync(c => accountDto.SelectedConsoles.Contains(c.Id)))
            .ReturnsAsync(consoles);
        
        //act
        await _accountService.UpdateAsync(accountDto);
        
        //assert
        Assert.That(account.Price, Is.EqualTo(accountDto.Price));
        Assert.That(account.Login, Is.EqualTo(accountDto.Login));
        Assert.That(account.Password, Is.EqualTo(accountDto.Password));
        Assert.That(account.Consoles, Is.EqualTo(consoles));
        _unitOfWorkMock.Verify(u=>u.SaveChangesAsync(),Times.Once);
        
    }
    
    [Test]
    public async Task UpdateAsync_AccountDtoIsRentedEqualFalse_UpdateAccount()
    {
        //arrange
        var accountDto = new AccountForUpdateDto()
        {   Id = 1, 
            ProductId = 1, 
            IsRented = false,
            Price = 100,
            Login ="TestLogin",
            Password = "TestPassword",
            SelectedConsoles = new []{1,2}
        };
        var order = new Order() { AccountId = 1, IsActive = true };
        List<Console> consoles = new List<Console>() { new Console() { Id = 1 }, new Console() { Id = 2 } };
        var account = new Account() { Id = 1, ProductId = 1 ,AccountNumber = 1, Name = "test",IsRented = true};
        var validationResult = new ValidationResult();
        
        _accountForUpdateDtoValidatorMock
            .Setup(u => u.ValidateAsync(accountDto,default))
            .ReturnsAsync(validationResult);
        _unitOfWorkMock
            .Setup(u => u.AccountRepository
                .GetByIdAsync(accountDto.Id, true,It.IsAny<Expression<Func<Account,object>>>()))
            .ReturnsAsync(account);
        _unitOfWorkMock
            .Setup(u => u.ConsoleRepository.GetConsolesAsync(x => accountDto.SelectedConsoles.Contains(x.Id)))
            .ReturnsAsync(consoles);
        _unitOfWorkMock
            .Setup(u => u.OrderRepository
                .FirstOrDefaultAsync(o => o.AccountId == account.Id && o.IsActive, true))
            .ReturnsAsync(order);
        
        //act
        await _accountService.UpdateAsync(accountDto);
        
        //assert
        Assert.That(account.IsRented, Is.EqualTo(false));
        Assert.That(order.IsActive, Is.EqualTo(false));
        _unitOfWorkMock.Verify(u=>u.SaveChangesAsync(),Times.Once);
        
    }
    
    [Test]
    public async Task UpdateAsync_NewProductId_UpdateAccountWithNewProductId()
    {
        //arrange
        var accountDto = new AccountForUpdateDto()
        {   Id = 1, 
            ProductId = 2, 
            IsRented = false,
            Price = 100,
            Login ="TestLogin",
            Password = "TestPassword",
            SelectedConsoles = new []{1,2}
        };
        int accountNumber = 1;
        List<Console> consoles = new List<Console>() { new Console() { Id = 1 }, new Console() { Id = 2 } };
        var account = new Account() { Id = 1, ProductId = 1 ,AccountNumber = 4, Name = "test",IsRented = false};
        var product = new Product() { Id = 2 , Name = "TestProductName"};
        var validationResult = new ValidationResult();
        
        _accountForUpdateDtoValidatorMock
            .Setup(u => u.ValidateAsync(accountDto,default))
            .ReturnsAsync(validationResult);
        _unitOfWorkMock
            .Setup(u => u.AccountRepository
                .GetByIdAsync(accountDto.Id, true,It.IsAny<Expression<Func<Account,object>>>()))
            .ReturnsAsync(account);
        _unitOfWorkMock
            .Setup(u => u.ConsoleRepository.GetConsolesAsync(x => accountDto.SelectedConsoles.Contains(x.Id)))
            .ReturnsAsync(consoles);
        _unitOfWorkMock
            .Setup(u => u.ProductRepository.GetByIdAsync(accountDto.ProductId, false))
            .ReturnsAsync(product);
        _unitOfWorkMock
            .Setup(u => u.AccountRepository.GetAccountsAsync(It.IsAny<Expression<Func<Account,bool>>>()))
            .ReturnsAsync(new List<Account>());
        
        //act
        await _accountService.UpdateAsync(accountDto);
        
        //assert
        Assert.That(account.AccountNumber, Is.EqualTo(accountNumber));
        Assert.That(account.Name,Is.EqualTo(product.Name));
        _unitOfWorkMock.Verify(u=>u.SaveChangesAsync(),Times.Once);
        
    }
    
    
    
    
}