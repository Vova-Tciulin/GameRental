using AutoMapper;
using GameRental.Application.DTO.Account;
using GameRental.Application.DTO.Category;
using GameRental.Application.DTO.Console;
using GameRental.Application.DTO.Product;
using GameRental.Application.Interfaces;
using GameRental.Web.Areas.Administration.Controllers;
using GameRental.Web.Models;
using GameRental.Web.Models.AccountModels;
using GameRental.Web.Models.ProductModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace WebTets.AdminControllersTests;

[TestFixture]
public class RentAccountControllerTests
{
    private Mock<IMapper> _mapper;
    private Mock<IServiceManager> _serviceManagerMoq;
    private RentAccountController _controller;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mock<IMapper>();
        _serviceManagerMoq = new Mock<IServiceManager>();
        _controller = new RentAccountController(_mapper.Object, _serviceManagerMoq.Object);
        _serviceManagerMoq.Setup(u => u.AccountService).Returns(new Mock<IAccountService>().Object);
        _serviceManagerMoq.Setup(u => u.ProductService).Returns(new Mock<IProductService>().Object);
        _serviceManagerMoq.Setup(u => u.ConsoleService).Returns(new Mock<IConsoleService>().Object);
    }
    
    [Test]
    public async Task Index_ReturnViewResultWithProducts()
    {
        var accountsDto = new List<AccountDto>() { new AccountDto(), new AccountDto() };
        var accountVm = new List<AccountVm>() { new AccountVm(), new AccountVm() };
        _mapper.Setup(u => u.Map<List<AccountVm>>(accountsDto)).Returns(accountVm);
        _serviceManagerMoq
            .Setup(u => u.AccountService.GetAllAsync(null,
                a=>a.Consoles,a=>a.Orders.Where(x=>x.IsActive==true)))
            .ReturnsAsync(accountsDto);
        
        //act
        var result = await _controller.Index();
        
        //assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<List<AccountVm>>(viewResult?.Model);
    }
    
    [Test]
    public async Task AddAccount_GET_ReturnsViewResult()
    {
        //arrange
        var productsDto = new List<ProductDTO>() { new ProductDTO(), new ProductDTO() };
        var productsVm = new List<ProductVM>() { new ProductVM(), new ProductVM() };
        var consolesDto = new List<ConsoleDto>() { new ConsoleDto(), new ConsoleDto() };
        var consolesVm = new List<ConsoleVm>() { new ConsoleVm(), new ConsoleVm() };
        _mapper.Setup(u => u.Map<List<ProductVM>>(productsDto)).Returns(productsVm);
        _mapper.Setup(u => u.Map<List<ConsoleVm>>(consolesDto)).Returns(consolesVm);
        _serviceManagerMoq.Setup(u => u.ProductService.GetProductsAsync(null)).ReturnsAsync(productsDto);
        _serviceManagerMoq.Setup(u => u.ConsoleService.GetAllAsync()).ReturnsAsync(consolesDto);

        // Act
        var result = await _controller.AddAccount();

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<AccountForAddVm>(viewResult?.Model);
        
    }
    
    [Test]
    public async Task AddAccount_POST_ValidModelState_RedirectsToIndex()
    {
        // Arrange
        var accountForAddVm = new AccountForAddVm()
            {Login = "test",Password = "pass",Price = 123,ProductId = 1,SelectedConsoles = new []{1,2}};
        var accountForAddDto = new AccountForAddDto()
            {Login = "test",Password = "pass",Price = 123,ProductId = 1,SelectedConsoles = new []{1,2}};
        _mapper.Setup(m => m.Map<AccountForAddDto>(accountForAddVm)).Returns(accountForAddDto);
        
        // Act
        var result = await _controller.AddAccount(accountForAddVm);

        // Assert
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        var redirectToAction = result as RedirectToActionResult;
        Assert.That(redirectToAction?.ActionName, Is.EqualTo("Index"));
        _serviceManagerMoq.Verify(u=>u.AccountService.AddAccountAsync(accountForAddDto), Times.Once);
    }
    
    [Test]
    public async Task AddAccount_POST_InvalidModelState_RedirectsToIndex()
    {
        // Arrange
        var productsDto = new List<ProductDTO>() { new ProductDTO(), new ProductDTO() };
        var productsVm = new List<ProductVM>() { new ProductVM(), new ProductVM() };
        var consolesDto = new List<ConsoleDto>() { new ConsoleDto(), new ConsoleDto() };
        var consolesVm = new List<ConsoleVm>() { new ConsoleVm(), new ConsoleVm() };
        _mapper.Setup(u => u.Map<List<ProductVM>>(productsDto)).Returns(productsVm);
        _mapper.Setup(u => u.Map<List<ConsoleVm>>(consolesDto)).Returns(consolesVm);
        _serviceManagerMoq.Setup(u => u.ProductService.GetProductsAsync(null)).ReturnsAsync(productsDto);
        _serviceManagerMoq.Setup(u => u.ConsoleService.GetAllAsync()).ReturnsAsync(consolesDto);
        var accountForAddVm = new AccountForAddVm();
        _controller.ModelState.AddModelError("error", "errorMsg");
        
        // Act
        var result = await _controller.AddAccount( accountForAddVm);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<AccountForAddVm>(viewResult?.Model);
    }
    
    
    [Test]
    public async Task Update_GET_ReturnsViewResult()
    {
        //assert
        int id = 1;
        var account = new AccountDto() { Id = 1 };
        var accountForUpdateVm = new AccountForUpdateVm() { Id = 1 };
        var productsDto = new List<ProductDTO>() { new ProductDTO(), new ProductDTO() };
        var productsVm = new List<ProductVM>() { new ProductVM(), new ProductVM() };
        var consolesDto = new List<ConsoleDto>() { new ConsoleDto(), new ConsoleDto() };
        var consolesVm = new List<ConsoleVm>() { new ConsoleVm(), new ConsoleVm() };
        _mapper.Setup(u => u.Map<AccountForUpdateVm>(account)).Returns(accountForUpdateVm);
        _mapper.Setup(u => u.Map<List<ProductVM>>(productsDto)).Returns(productsVm);
        _mapper.Setup(u => u.Map<List<ConsoleVm>>(consolesDto)).Returns(consolesVm);
        _serviceManagerMoq.Setup(u => u.AccountService.GetByIdAsync(id, a => a.Consoles)).ReturnsAsync(account);
        _serviceManagerMoq.Setup(u => u.ProductService.GetProductsAsync(null)).ReturnsAsync(productsDto);
        _serviceManagerMoq.Setup(u => u.ConsoleService.GetAllAsync()).ReturnsAsync(consolesDto);
        // Act
        var result = await _controller.Update(id);
        
        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<AccountForUpdateVm>(viewResult?.Model);
    }
    
    [Test]
    public async Task Update_POST_ValidModelState_RedirectsToIndex()
    {
        // Arrange
        var accountForUpdateVm = new AccountForUpdateVm()
            { Id=1, Login = "test",Password = "pass",Price = 123,ProductId = 1,SelectedConsoles = new []{1,2}};
        var accountForUpdateDto = new AccountForUpdateDto()
            {Id=1,Login = "test",Password = "pass",Price = 123,ProductId = 1,SelectedConsoles = new []{1,2}};
        _mapper.Setup(m => m.Map<AccountForUpdateDto>(accountForUpdateVm )).Returns(accountForUpdateDto );
        
        // Act
        var result = await _controller.Update(accountForUpdateVm);

        // Assert
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        var redirectToAction = result as RedirectToActionResult;
        Assert.That(redirectToAction?.ActionName, Is.EqualTo("Index"));
        _serviceManagerMoq.Verify(u=>u.AccountService.UpdateAsync(accountForUpdateDto), Times.Once);
    }
    
    [Test]
    public async Task Update_POST_InvalidModelState_RedirectsToIndex()
    {
        // Arrange
        var productsDto = new List<ProductDTO>() { new ProductDTO(), new ProductDTO() };
        var productsVm = new List<ProductVM>() { new ProductVM(), new ProductVM() };
        var consolesDto = new List<ConsoleDto>() { new ConsoleDto(), new ConsoleDto() };
        var consolesVm = new List<ConsoleVm>() { new ConsoleVm(), new ConsoleVm() };
        _mapper.Setup(u => u.Map<List<ProductVM>>(productsDto)).Returns(productsVm);
        _mapper.Setup(u => u.Map<List<ConsoleVm>>(consolesDto)).Returns(consolesVm);
        _serviceManagerMoq.Setup(u => u.ProductService.GetProductsAsync(null)).ReturnsAsync(productsDto);
        _serviceManagerMoq.Setup(u => u.ConsoleService.GetAllAsync()).ReturnsAsync(consolesDto);
        var accountForUpdateVm = new AccountForUpdateVm();
        _controller.ModelState.AddModelError("error", "errorMsg");
        
        // Act
        var result = await _controller.Update( accountForUpdateVm);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<AccountForUpdateVm>(viewResult?.Model);
    }

    [Test]
    public async Task Delete_CallsCategoryService_ReturnOk()
    {
        int id = 1;
        
        //act
        var result=await _controller.Delete(id);
        
        //assert
        Assert.IsInstanceOf<OkResult>(result);
        _serviceManagerMoq.Verify(u=>u.AccountService.RemoveAsync(id),Times.Once);
    }
    
}