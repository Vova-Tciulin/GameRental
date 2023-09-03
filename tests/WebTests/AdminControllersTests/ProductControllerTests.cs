using AutoMapper;

using GameRental.Application.DTO.Category;
using GameRental.Application.DTO.Product;
using GameRental.Application.Interfaces;
using GameRental.Web.Areas.Administration.Controllers;
using GameRental.Web.Models;
using GameRental.Web.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace WebTets.AdminControllersTests;

[TestFixture]
public class ProductControllerTests
{
    private Mock<IMapper> _mapper;
    private Mock<IServiceManager> _serviceManagerMoq;
    private ProductController _controller;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mock<IMapper>();
        _serviceManagerMoq = new Mock<IServiceManager>();
        _controller = new ProductController(_mapper.Object, _serviceManagerMoq.Object);
        _serviceManagerMoq.Setup(u => u.ProductService).Returns(new Mock<IProductService>().Object);
        _serviceManagerMoq.Setup(u => u.ProductCategoryService).Returns(new Mock<IProductCategoryService>().Object);
    }
    
    [Test]
    public async Task Index_ReturnViewResultWithProducts()
    {
        var productsDto = new List<ProductDTO>() { new ProductDTO(), new ProductDTO() };
        var productsVm = new List<ProductVM>() { new ProductVM(), new ProductVM() };
        _mapper.Setup(u => u.Map<List<ProductVM>>(productsDto)).Returns(productsVm);
        _serviceManagerMoq.Setup(u => u.ProductService.GetProductsAsync(null,p=>p.ProductCategories)).ReturnsAsync(productsDto);
        
        //act
        var result = await _controller.Index();
        
        //assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<List<ProductVM>>(viewResult?.Model);
    }
    
    [Test]
    public async Task AddProduct_GET_ReturnsViewResult()
    {
        //arrange
        var categories = new List<ProductCategoryDTO>() { new ProductCategoryDTO(), new ProductCategoryDTO() };
        var categoriesVm = new List<ProductCategoryVM>() { new ProductCategoryVM(), new ProductCategoryVM() };
        _mapper.Setup(u => u.Map<List<ProductCategoryVM>>(categories)).Returns(categoriesVm);
        _serviceManagerMoq.Setup(u => u.ProductCategoryService.GetCategoriesAsync()).ReturnsAsync(categories);
        
        
        // Act
        var result = await _controller.AddProduct();

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<ProductAddVM>(viewResult?.Model);
        
    }
    
    [Test]
    public async Task AddProduct_POST_ValidModelState_RedirectsToIndex()
    {
        // Arrange
       
        var productAddVm = new ProductAddVM()
            {
                Description = "testDescription", 
                ImageCollection = new List<IFormFile>(){new FormFile(null,0,0,"image.png","image.png")},
                Name = "Name", Translate = "Rus", Year = 1111, ProductCategoriesId = new []{1,2}, TransitTime = 10
            };
        var productAdddto = new ProductAddDto()
        {
            Description = "testDescription",
            ImageCollection = new List<IFormFile>() { new FormFile(null, 0, 0, "image.png", "image.png") },
            Name = "Name", Translate = "Rus", Year = 1111, ProductCategoriesId = new[] { 1, 2 }, TransitTime = 10
        };
        
        _mapper.Setup(m => m.Map<ProductAddDto>(productAddVm)).Returns(productAdddto);
        
        // Act
        var result = await _controller.AddProduct(productAddVm);

        // Assert
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        var redirectToAction = result as RedirectToActionResult;
        Assert.That(redirectToAction?.ActionName, Is.EqualTo("Index"));
        _serviceManagerMoq.Verify(u=>u.ProductService.AddProductAsync(productAdddto), Times.Once);
    }
    
    [Test]
    public async Task AddProduct_POST_InvalidModelState_RedirectsToIndex()
    {
        // Arrange
        var categories = new List<ProductCategoryDTO>() { new ProductCategoryDTO(), new ProductCategoryDTO() };
        var categoriesVm = new List<ProductCategoryVM>() { new ProductCategoryVM(), new ProductCategoryVM() };
        _mapper.Setup(u => u.Map<List<ProductCategoryVM>>(categories)).Returns(categoriesVm);
        _serviceManagerMoq.Setup(u => u.ProductCategoryService.GetCategoriesAsync()).ReturnsAsync(categories);
        var productAddVm = new ProductAddVM();
        _controller.ModelState.AddModelError("error", "errorMsg");
        
        // Act
        var result = await _controller.AddProduct(productAddVm);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<ProductAddVM>(viewResult?.Model);
    }
    
     [Test]
    public async Task Update_GET_ReturnsViewResult()
    {
        //assert
        int id = 1;
        var productDto = new ProductDTO();
        var productVm = new ProductForUpdateVm();
        var categories = new List<ProductCategoryDTO>() { new ProductCategoryDTO(), new ProductCategoryDTO() };
        var categoriesVm = new List<ProductCategoryVM>() { new ProductCategoryVM(), new ProductCategoryVM() };
        _mapper.Setup(u => u.Map<List<ProductCategoryVM>>(categories)).Returns(categoriesVm);
        _mapper.Setup(u => u.Map<ProductForUpdateVm>(productDto)).Returns(productVm);
        _serviceManagerMoq.Setup(u => u.ProductCategoryService.GetCategoriesAsync()).ReturnsAsync(categories);
        _serviceManagerMoq
            .Setup(u => u.ProductService.GetProductAsync(id, p => p.ProductCategories,
                    p => p.Images))
            .ReturnsAsync(productDto);
        // Act
        var result = await _controller.Update(id);
        
        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<ProductForUpdateVm>(viewResult?.Model);
    }
    
    [Test]
    public async Task Update_POST_ValidModelState_RedirectsToIndex()
    {
        // Arrange
       
        var productUpdateVm = new ProductForUpdateVm()
        {
            Id=1, Description = "testDescription", 
            ImageCollection = new List<IFormFile>(){new FormFile(null,0,0,"image.png","image.png")},
            Name = "Name", Translate = "Rus", Year = 1111, ProductCategoriesId = new []{1,2}, TransitTime = 10
        };
        var productUpdateDto = new ProductForUpdateDto()
        {
            Id=1,Description = "testDescription",
            ImageCollection = new List<IFormFile>() { new FormFile(null, 0, 0, "image.png", "image.png") },
            Name = "Name", Translate = "Rus", Year = 1111, ProductCategoriesId = new[] { 1, 2 }, TransitTime = 10
        };
        
        _mapper.Setup(m => m.Map<ProductForUpdateDto>(productUpdateVm)).Returns(productUpdateDto);
        
        // Act
        var result = await _controller.Update(productUpdateVm);

        // Assert
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        var redirectToAction = result as RedirectToActionResult;
        Assert.That(redirectToAction?.ActionName, Is.EqualTo("Index"));
        _serviceManagerMoq.Verify(u=>u.ProductService.UpdateProductAsync(productUpdateDto), Times.Once);
    }
    
    [Test]
    public async Task Update_POST_InvalidModelState_RedirectsToIndex()
    {
        // Arrange
        var categories = new List<ProductCategoryDTO>() { new ProductCategoryDTO(), new ProductCategoryDTO() };
        var categoriesVm = new List<ProductCategoryVM>() { new ProductCategoryVM(), new ProductCategoryVM() };
        _mapper.Setup(u => u.Map<List<ProductCategoryVM>>(categories)).Returns(categoriesVm);
        _serviceManagerMoq.Setup(u => u.ProductCategoryService.GetCategoriesAsync()).ReturnsAsync(categories);
        var productForUpdateVm = new ProductForUpdateVm();
        _controller.ModelState.AddModelError("error", "errorMsg");
        
        // Act
        var result = await _controller.Update(productForUpdateVm);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<ProductForUpdateVm>(viewResult?.Model);
    }

    [Test]
    public async Task Delete_CallsCategoryService_ReturnOk()
    {
        int id = 1;
        
        //act
        var result=await _controller.Delete(id);
        
        //assert
        Assert.IsInstanceOf<OkResult>(result);
        _serviceManagerMoq.Verify(u=>u.ProductService.RemoveProductAsync(id),Times.Once);
    }
}