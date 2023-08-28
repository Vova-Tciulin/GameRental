using System.Linq.Expressions;
using AutoMapper;
using EmptyFiles;
using FluentValidation;
using FluentValidation.Results;
using GameRental.Application.DTO.Account;
using GameRental.Application.DTO.Product;
using GameRental.Application.Exceptions;
using GameRental.Application.Interfaces;
using GameRental.Application.Services;
using GameRental.Domain.Entities;
using GameRental.Domain.Exceptions;
using GameRental.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;

namespace ApplicationTests.ServicesUnitTests;

[TestFixture]
public class ProductServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private Mock<IMapper> _mapperMock = null!;
    private Mock<IValidator<ProductAddDto>> _productForAddDtoValidatorMock=null!;
    private Mock<IValidator<ProductForUpdateDto>> _productForUpdateDtoValidatorMock=null!;
    private Mock<IImageOperations> _imageOperationsMock = null!;
    private Mock<IProductRepository> _productRepository = null!;
    private Mock<IConsoleRepository> _consoleRepository = null!;
    private Mock<IImageRepository> _imageRepository = null!;
    private ProductService _productService=null!;
    
    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _productRepository= new Mock<IProductRepository>();
        _consoleRepository= new Mock<IConsoleRepository>();
        _productForAddDtoValidatorMock = new Mock<IValidator<ProductAddDto>>();
        _productForUpdateDtoValidatorMock = new Mock<IValidator<ProductForUpdateDto>>();
        _imageOperationsMock = new Mock<IImageOperations>();
        _imageRepository = new Mock<IImageRepository>();
        
        _productService = new ProductService(
            _unitOfWorkMock.Object,_mapperMock.Object,
            _productForUpdateDtoValidatorMock.Object,
            _productForAddDtoValidatorMock.Object,
            _imageOperationsMock.Object);

       
        _unitOfWorkMock.Setup(u => u.ProductRepository).Returns(_productRepository.Object);
        _unitOfWorkMock.Setup(u => u.ConsoleRepository).Returns(_consoleRepository.Object);
        _unitOfWorkMock.Setup(u => u.ImageRepository).Returns(_imageRepository.Object);
    }

    [Test]
    public async Task AddProductAsync_ValidProduct_AddProduct()
    {
        var productAddDto = new ProductAddDto()
        {
            Description = "description", Name = "Test Name",
            ProductCategoriesId = new[] { 1, 2 },
            ImageCollection = new List<IFormFile>() { new FormFile(null, 0, 0, "image.png", "image.png") }
        };
        var product = new Product() { Description = "description", Name = "Test Name" };
        var categories = new List<ProductCategory>()
            { new ProductCategory() { Id = 1 }, new ProductCategory() { Id = 2 } };
        var image = new Image() { ImgName = "image.png", Product = product };
        var validationResult = new ValidationResult();
        _productForAddDtoValidatorMock
            .Setup(v => v.ValidateAsync(productAddDto,default))
            .ReturnsAsync(validationResult);
        _mapperMock.Setup(u => u.Map<Product>(productAddDto)).Returns(product);
        _unitOfWorkMock
            .Setup(u => u.ProductCategoryRepository
                .GetAllAsync(x => productAddDto.ProductCategoriesId.Contains(x.Id)))
            .ReturnsAsync(categories);
        
        //act
        await _productService.AddProductAsync(productAddDto);
        
        //assert
        Assert.That(product.Images, Has.Count.EqualTo(1));
        Assert.That(product.ProductCategories,Is.EqualTo(categories));
        _unitOfWorkMock.Verify(u=>u.ProductRepository.Add(product),Times.Once);
        _unitOfWorkMock.Verify(u=>u.ImageRepository.AddRange(product.Images),Times.Once);
        _unitOfWorkMock.Verify(u=>u.SaveChangesAsync(),Times.Once);
    }

    [Test]
    public void AddProductAsync_InvalidValidation_ThrowNotValidateModelException()
    {
        var productAddDto = new ProductAddDto();
        var validationFailure = new ValidationFailure("test", "Error message test");
        var validationResult = new ValidationResult(new []{validationFailure});
        _productForAddDtoValidatorMock
            .Setup(u => u.ValidateAsync(productAddDto,default))
            .ReturnsAsync(validationResult);
        
        //act && assert
        Assert.ThrowsAsync<NotValidateModelException>(() => _productService.AddProductAsync(productAddDto));
    }

    [Test]
    public async Task GetProductsAsync_ReturnProductsDto()
    {
        var products = new List<Product>() { new Product(), new Product(), new Product() };
        var productsDto = new List<ProductDTO>() { new ProductDTO(), new ProductDTO(), new ProductDTO() } as IEnumerable<ProductDTO>;
        _mapperMock.Setup(u => u.Map<IEnumerable<ProductDTO>>(products)).Returns(productsDto);
        _unitOfWorkMock.Setup(u => u.ProductRepository.GetAllAsync(null)).ReturnsAsync(products);
        
        //act
        var result = await _productService.GetProductsAsync();
        
        //assert
        Assert.That(result,Is.EqualTo(productsDto));
    }

    [Test]
    public async Task GetProductAsync_ProductExist_ReturnProductDto()
    {
        int id = 1;
        var product = new Product() { Id = id };
        var productDto = new ProductDTO() { Id = id };
        _unitOfWorkMock.Setup(u => u.ProductRepository.GetByIdAsync(id, false)).ReturnsAsync(product);
        _mapperMock.Setup(u => u.Map<ProductDTO>(product)).Returns(productDto);
        
        //act
        var result = await _productService.GetProductAsync(id);
        
        //assert
        Assert.That(result,Is.EqualTo(productDto));
    }
    
    [Test]
    public void GetProductAsync_ProductNotExist_ThrowProductNotFoundException()
    {
        int id = 1;
        _unitOfWorkMock.Setup(u => u.ProductRepository.GetByIdAsync(id, false)).ReturnsAsync((Product)null);
        
        //act && assert
        Assert.ThrowsAsync<ProductNotFoundExeption>(() => _productService.GetProductAsync(id));
    }
    

    [Test]
    public async Task RemoveProductAsync()
    {
        var id = 1;
        var image = new Image();
        var product = new Product() { Id = 1, Images = new List<Image>(){image}};
        _unitOfWorkMock
            .Setup(u => u.ProductRepository.GetByIdAsync(id, true, p => p.Images))
            .ReturnsAsync(product);
        
        //act
        await _productService.RemoveProductAsync(id);
        
        //assert
        _unitOfWorkMock.Verify(u=>u.ProductRepository.Remove(product),Times.Once);
        _unitOfWorkMock.Verify(u=>u.SaveChangesAsync(),Times.Once);
    }
    
    [Test]
    public void RemoveProductAsync_ProductNotExist_ThrowProductNotFoundException()
    {
        int id = 1;
        _unitOfWorkMock.Setup(u => u.ProductRepository.GetByIdAsync(id, false)).ReturnsAsync((Product)null);
        
        //act && assert
        Assert.ThrowsAsync<ProductNotFoundExeption>(() => _productService.RemoveProductAsync(id));
    }

    [Test]
    public async Task UpdateProductAsync_ValidProduct_UpdateProduct()
    {
        var productForUpdateDto = new ProductForUpdateDto()
        {
            Id = 1, Description = "test", ProductCategoriesId = new[] { 1, 2 },
            Name = "TestName", Translate = "Eng", Year = 2000,
            ImagesId = new[] { 1}, ImageCollection = { new FormFile(null, 0, 0, "image.png", "image.png") }
        };
        var removeImages = new List<Image>() { new Image() { Id = 1 }, new Image() { Id = 3 } };
        var product = new Product() { Id = 1, Images = removeImages};
        var validationResult = new ValidationResult();
        var categories = new List<ProductCategory>()
            { new ProductCategory() { Id = 1 }, new ProductCategory() { Id = 2 } };

        _productForUpdateDtoValidatorMock
            .Setup(u => u.ValidateAsync(productForUpdateDto,default))
            .ReturnsAsync(validationResult);
        _unitOfWorkMock
            .Setup(u => u.ProductRepository
                .GetByIdAsync(productForUpdateDto.Id, true, p=>p.ProductCategories,p=>p.Images))
            .ReturnsAsync(product);
        _unitOfWorkMock
            .Setup(u => u.ProductCategoryRepository.GetAllAsync(x =>
                productForUpdateDto.ProductCategoriesId.Contains(x.Id)))
            .ReturnsAsync(categories);
        
        //act
        await _productService.UpdateProductAsync(productForUpdateDto);
        
        //assert
        Assert.That(product.Images, Has.Count.EqualTo(2));
        Assert.That(product.ProductCategories, Is.EqualTo(categories));
        _unitOfWorkMock.Verify(u=>u.SaveChangesAsync(),Times.Once);
    }

    [Test]
    public void UpdateProductAsync_InvalidProduct_ThrowNotValidateModelException()
    {
        var productForUpdate = new ProductForUpdateDto();
        var validationFailure = new ValidationFailure("test", "Error message test");
        var validationResult = new ValidationResult(new []{validationFailure});
        _productForUpdateDtoValidatorMock
            .Setup(u => u.ValidateAsync(productForUpdate,default))
            .ReturnsAsync(validationResult);
        
        //act && assert
        Assert.ThrowsAsync<NotValidateModelException>(() => _productService.UpdateProductAsync(productForUpdate));
    }
    
}