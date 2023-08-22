using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using GameRental.Application.DTO.Account;
using GameRental.Application.Interfaces;
using GameRental.Application.Services;
using GameRental.Domain.Entities;
using GameRental.Domain.Exceptions;
using GameRental.Domain.IRepository;
using Moq;

namespace ApplicationTests.ServicesUnitTests;

public class AccountServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private Mock<IMapper> _mapperMock = null!;
    private Mock<IImageOperations> _imageOperationsMock = null!;
    private Mock<IValidator<AccountForAddDto>> _accountForAddDtoValidatorMock;
    private Mock<IValidator<AccountForUpdateDto>> _accountForUpdateDtoValidatorMock;
    private Mock<IAccountRepository> _accountRepository = null!;
    private AccountService _accountService=null!;
    
    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _accountRepository= new Mock<IAccountRepository>();
        _imageOperationsMock = new Mock<IImageOperations>();
        _accountForAddDtoValidatorMock = new Mock<IValidator<AccountForAddDto>>();
        _accountForUpdateDtoValidatorMock = new Mock<IValidator<AccountForUpdateDto>>();
        
        _accountService = new AccountService(
            _unitOfWorkMock.Object,_mapperMock.Object,
            _accountForAddDtoValidatorMock.Object,_accountForUpdateDtoValidatorMock.Object);

        _unitOfWorkMock.Setup(u => u.AccountRepository).Returns(_accountRepository.Object);
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
    public async Task AddAccountAsync_ValidAccount_AddAccount()
    {
        //arrange
        var accountDto = new AccountForAddDto();
        var account = new AccountForAddDto();
        var validationResult = new FluentValidation.Results.ValidationResult();
        _accountForAddDtoValidatorMock.Setup(v => v.ValidateAsync(accountDto,default)).ReturnsAsync(validationResult);
        
    }
    
    

    
    
}