using AutoMapper;
using GameRental.Application.DTO.Category;
using GameRental.Application.Interfaces;
using GameRental.Application.Services;
using GameRental.Domain.Entities;
using GameRental.Domain.Exceptions;
using GameRental.Domain.IRepository;
using Microsoft.AspNetCore.Http.Internal;
using Moq;

namespace ApplicationTests.ServicesUnitTests;

[TestFixture]
public class ProductCategoryServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private Mock<IMapper> _mapperMock = null!;
    private Mock<IImageOperations> _imageOperationsMock = null!;
    private Mock<IProductCategoryRepository> _productCategoryRepository = null!;
    private ProductCategoryService _productCategoryService=null!;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _productCategoryRepository = new Mock<IProductCategoryRepository>();
        _imageOperationsMock = new Mock<IImageOperations>();
        _productCategoryService = new ProductCategoryService(_unitOfWorkMock.Object, _mapperMock.Object,_imageOperationsMock.Object);

        _unitOfWorkMock.Setup(u => u.ProductCategoryRepository).Returns(_productCategoryRepository.Object);
    }

    [Test]
    public void GetCategoryByIdAsync_CategoryDoesntExist_ReturnCategoryNotFoundException()
    {
        //arrange
        var categoryId = 1;
        _unitOfWorkMock.Setup(u => u.ProductCategoryRepository.GetByIdAsync(categoryId)).ReturnsAsync((ProductCategory)null);
        
        //act & assert
        Assert.ThrowsAsync<CategoryNotFoundException>(() => _productCategoryService.GetCategoryByIdAsync(categoryId));
    }
    
    [Test]
    public async Task GetCategoryByIdAsync_CategoryExist_ReturnCategoryDto()
    {
        //arrange
        var categoryId = 1;
        var category = new ProductCategory();
        var categoryDto = new ProductCategoryDTO();
        _unitOfWorkMock.Setup(u => u.ProductCategoryRepository.GetByIdAsync(categoryId)).ReturnsAsync(category);
        _mapperMock.Setup(u => u.Map<ProductCategoryDTO>(category)).Returns(categoryDto);
        
        //act
        var result = await _productCategoryService.GetCategoryByIdAsync(categoryId);
        
        //act & assert
        Assert.That(result,Is.EqualTo(categoryDto));
    }

    [Test]
    public async Task AddCategoryAsync_ValidCategory_CallsRepositoryAndImageOperations()
    {
        //arrange
        var categoryDto = new CategoryForUpsertDto()
        {
            Image = new FormFile(null,0,0,"image.png","image.png")
        };
        var category = new ProductCategory();
        _mapperMock.Setup(u => u.Map<ProductCategory>(categoryDto)).Returns(category);
        
        //act
        await _productCategoryService.AddCategoryAsync(categoryDto);
        
        //assert
        _mapperMock.Verify(u=>u.Map<ProductCategory>(categoryDto),Times.Once);
        _imageOperationsMock.Verify(u=>u.SaveImagesAsync(categoryDto.Image),Times.Once);
        _unitOfWorkMock.Verify(u=>u.ProductCategoryRepository.Add(category),Times.Once);
        _unitOfWorkMock.Verify(u=>u.SaveChangesAsync(),Times.Once);
    }

    [Test]
    public async Task GetCategoriesAsync_ReturnsCategoriesDto()
    {
        //arrange
        var categories = new List<ProductCategory>()
            { new ProductCategory(), new ProductCategory(), new ProductCategory(), };
        var categoriesDto = new List<ProductCategoryDTO>()
            { new ProductCategoryDTO(), new ProductCategoryDTO(), new ProductCategoryDTO(), };
        _unitOfWorkMock.Setup(u => u.ProductCategoryRepository.GetAllAsync(null)).ReturnsAsync(categories);
        _mapperMock.Setup(u => u.Map<IEnumerable<ProductCategoryDTO>>(categories)).Returns(categoriesDto);
        
        //act
        var result = await _productCategoryService.GetCategoriesAsync();
        
        //assert
        Assert.That(result,Is.EqualTo(categoriesDto));

    }

    [Test]
    public async Task UpdateProductCategoryAsync_CategoryExistAndImageIsNull_UpdateCategory()
    {
        //arrange
        var categoryDto = new CategoryForUpsertDto() { Id = 1, Image = null, Name = "updatedName" };
        var category = new ProductCategory() { Id = 1, ImgPath = "test.jpeg", Name="OldName" };
        _unitOfWorkMock.Setup(u => u.ProductCategoryRepository.GetByIdAsync(categoryDto.Id)).ReturnsAsync(category);

        //act
        await _productCategoryService.UpdateProductCategoryAsync(categoryDto);
        
        //assert
        Assert.That(category.Name,Is.EqualTo(categoryDto.Name));
        _unitOfWorkMock.Verify(u => u.ProductCategoryRepository.Update(category), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
    
    [Test]
    public async Task UpdateProductCategoryAsync_ExistingCategoryAndImageNotNull_UpdatesCategory()
    {
        // Arrange
        var categoryId = 1;
        var categoryDto = new CategoryForUpsertDto
        {
            Id = categoryId,
            Name = "UpdatedCategory",
            Image = new FormFile(null, 0, 0, "updated-image.png", "updated-image.png")
        };
        
        var existingCategory = new ProductCategory { Id = categoryId, Name = "OldCategory", ImgPath = "old-image.png" };
        _unitOfWorkMock.Setup(u => u.ProductCategoryRepository.GetByIdAsync(categoryId)).ReturnsAsync(existingCategory);
        
        // Act
        await _productCategoryService.UpdateProductCategoryAsync(categoryDto);

        // Assert
        Assert.That(existingCategory.Name, Is.EqualTo(categoryDto.Name));
        Assert.That(existingCategory.ImgPath, Is.EqualTo(categoryDto.Image.FileName));
        _imageOperationsMock.Verify(io => io.RemoveImages("old-image.png"));
        _imageOperationsMock.Verify(io => io.SaveImagesAsync(categoryDto.Image), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.ProductCategoryRepository.Update(existingCategory), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
    
    
    [Test]
    public void UpdateProductCategoryAsync_CategoryDoesntExist_ReturnCategoryNotFoundException()
    {
        //arrange
        var categoryDto = new CategoryForUpsertDto() { Id = 1 };
        _unitOfWorkMock.Setup(u => u.ProductCategoryRepository.GetByIdAsync(categoryDto.Id)).ReturnsAsync((ProductCategory)null);
        
        //act & assert
        Assert.ThrowsAsync<CategoryNotFoundException>(() => _productCategoryService.UpdateProductCategoryAsync(categoryDto));
    }

    [Test]
    public void RemoveProductCategoryAsync_InvalidCategory_ReturnCategoryNotFoundException()
    {
        //arrange
        var categoryId = 1;
        _unitOfWorkMock.Setup(u => u.ProductCategoryRepository.GetByIdAsync(categoryId)).ReturnsAsync((ProductCategory)null);
        
        //act & assert
        Assert.ThrowsAsync<CategoryNotFoundException>(() => _productCategoryService.RemoveProductCategoryAsync(categoryId));
    }
    
    [Test]
    public async Task RemoveProductCategoryAsync_ValidCategory_CallsRepositoryAndImageOperations()
    {
        //arrange
        var categoryId = 1;
        var category = new ProductCategory() { Id = 1, ImgPath = "test.jpg" };
        _unitOfWorkMock.Setup(u => u.ProductCategoryRepository.GetByIdAsync(categoryId)).ReturnsAsync(category);

        //act
        await _productCategoryService.RemoveProductCategoryAsync(categoryId);
        
        //assert
        _imageOperationsMock.Verify(u=>u.RemoveImages(category.ImgPath),Times.Once);
        _unitOfWorkMock.Verify(u=>u.ProductCategoryRepository.Remove(category),Times.Once);
        _unitOfWorkMock.Verify(u=>u.SaveChangesAsync(),Times.Once);
    }

    
    
}