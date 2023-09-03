using AutoMapper;
using GameRental.Application.DTO.Category;
using GameRental.Application.Interfaces;
using GameRental.Application.Services;
using GameRental.Web.Areas.Administration.Controllers;
using GameRental.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace WebTets.AdminControllersTests;

[TestFixture]
public class CategoryControllerTests
{
    private Mock<IMapper> _mapper;
    private Mock<IServiceManager> _serviceManagerMoq;
    private CategoryController _controller;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mock<IMapper>();
        _serviceManagerMoq = new Mock<IServiceManager>();
        _controller = new CategoryController(_mapper.Object, _serviceManagerMoq.Object);
        _serviceManagerMoq.Setup(u => u.ProductCategoryService).Returns(new Mock<IProductCategoryService>().Object);
    }

    [Test]
    public async Task Index_ReturnViewResultWithCategories()
    {
        var categories = new List<ProductCategoryDTO>() { new ProductCategoryDTO(), new ProductCategoryDTO() };
        var categoriesVm = new List<ProductCategoryVM>() { new ProductCategoryVM(), new ProductCategoryVM() };
        _mapper.Setup(u => u.Map<List<ProductCategoryVM>>(categories)).Returns(categoriesVm);
        _serviceManagerMoq.Setup(u => u.ProductCategoryService.GetCategoriesAsync()).ReturnsAsync(categories);
        
        //act
        var result = await _controller.Index();
        
        //assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<List<ProductCategoryVM>>(viewResult?.Model);

    }
    
    [Test]
    public void AddCategory_GET_ReturnsViewResult()
    {
        // Act
        var result = _controller.AddCategory();

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
    }
    
    [Test]
    public async Task AddCategory_POST_ValidModelState_RedirectsToIndex()
    {
        // Arrange
        var categoryVm = new CategoryForUpsertVm ()
            { Id = 0, Image = new FormFile(null,0,0,"image.png","image.png"), Name = "TestName"};
        var categoryDto = new CategoryForUpsertDto (){ Id = 0, Image = new FormFile(null,0,0,"image.png","image.png"), Name = "TestName"};
        _mapper.Setup(m => m.Map<CategoryForUpsertDto>(categoryVm)).Returns(categoryDto);
        
        // Act
        var result = await _controller.AddCategory(categoryVm);

        // Assert
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        var redirectToAction = result as RedirectToActionResult;
        Assert.That(redirectToAction?.ActionName, Is.EqualTo("Index"));
        _serviceManagerMoq.Verify(
            u => u.ProductCategoryService.AddCategoryAsync(It.IsAny<CategoryForUpsertDto>()), Times.Once);
    }
    
    [Test]
    public async Task AddCategory_POST_InvalidModelState_RedirectsToIndex()
    {
        // Arrange
        var categoryVm = new CategoryForUpsertVm();
        
        // Act
        var result = await _controller.AddCategory(categoryVm);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<CategoryForUpsertVm>(viewResult?.Model);
       
    }
    
    [Test]
    public async Task Update_GET_ReturnsViewResult()
    {
        //assert
        int id = 1;
        var categoryDto = new ProductCategoryDTO() { Id = 1 };
        var categoryVm = new CategoryForUpsertVm() { Id = 1 };
        _serviceManagerMoq.Setup(u => u.ProductCategoryService.GetCategoryByIdAsync(id)).ReturnsAsync(categoryDto);
        _mapper.Setup(u => u.Map<CategoryForUpsertVm>(categoryDto)).Returns(categoryVm);
        // Act
        var result = await _controller.Update(id);
        
        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<CategoryForUpsertVm>(viewResult?.Model);
    }
    
    [Test]
    public async Task Update_POST_ValidModelState_RedirectsToIndex()
    {
        // Arrange
        var categoryVm = new CategoryForUpsertVm ()
            { Id = 1, Image = new FormFile(null,0,0,"image.png","image.png"), Name = "TestName"};
        var categoryDto = new CategoryForUpsertDto (){ Id = 1, Image = new FormFile(null,0,0,"image.png","image.png"), Name = "TestName"};
        _mapper.Setup(m => m.Map<CategoryForUpsertDto>(categoryVm)).Returns(categoryDto);
        
        // Act
        var result = await _controller.Update(categoryVm);

        // Assert
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        var redirectToAction = result as RedirectToActionResult;
        Assert.That(redirectToAction?.ActionName, Is.EqualTo("Index"));
        _serviceManagerMoq.Verify(
            u=>u.ProductCategoryService.UpdateProductCategoryAsync(It.IsAny<CategoryForUpsertDto>()),Times.Once);
    }
    
    [Test]
    public async Task Update_POST_InvalidModelState_RedirectsToIndex()
    {
        // Arrange
        var categoryVm = new CategoryForUpsertVm();
        _controller.ModelState.AddModelError("error", "errorMsg");
        
        // Act
        var result = await _controller.Update(categoryVm);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<CategoryForUpsertVm>(viewResult?.Model);
    }

    [Test]
    public async Task Delete_CallsCategoryService_ReturnOk()
    {
        int id = 1;
        
        //act
        var result=await _controller.Delete(id);
        
        //assert
        Assert.IsInstanceOf<OkResult>(result);
        _serviceManagerMoq.Verify(u=>u.ProductCategoryService.RemoveProductCategoryAsync(id),Times.Once);
    }
}