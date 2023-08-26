using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using GameRental.Application.DTO;
using GameRental.Application.DTO.Product;
using GameRental.Application.Exceptions;
using GameRental.Application.Interfaces;
using GameRental.Domain.Entities;
using GameRental.Domain.Exceptions;
using GameRental.Domain.IRepository;
using Microsoft.AspNetCore.Http;

namespace GameRental.Application.Services;

public class ProductService:IProductService
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _map;
    private readonly IValidator<ProductAddDto> _productAddDtoValidator;
    private readonly IValidator<ProductForUpdateDto> _productForUpdateDtoValidator;
    private readonly IImageOperations _imageOperations;

    public ProductService(IUnitOfWork db, IMapper map,
        IValidator<ProductForUpdateDto> productForUpdateDtoValidator,
        IValidator<ProductAddDto> productAddDtoValidator, IImageOperations imageOperations)
    {
        _db = db;
        _map = map;
        _productForUpdateDtoValidator = productForUpdateDtoValidator;
        _productAddDtoValidator = productAddDtoValidator;
        _imageOperations = imageOperations;
    }

    public async Task AddProductAsync(ProductAddDto productAddDto)
    {
        var validationResult = await _productAddDtoValidator.ValidateAsync(productAddDto);
        if (!validationResult.IsValid)
        {
            throw new NotValidateModelException(nameof(ProductDTO), validationResult.ToString("\n"));
        }
        
        var product = _map.Map<Product>(productAddDto);
        var categories= await _db.ProductCategoryRepository
            .GetAllAsync(u => productAddDto.ProductCategoriesId.Contains(u.Id));
        
        product.ProductCategories = categories.ToList();

        await _imageOperations.SaveImagesAsync(productAddDto.ImageCollection.ToArray());
        
        foreach (var image in productAddDto.ImageCollection)
        {
            product.Images.Add(new Image
            {
                ImgName = image.FileName,
                Product = product
            });
        }

        _db.ProductRepository.Add(product);
        _db.ImageRepository.AddRange(product.Images);
       
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsAsync(
        Expression<Func<Product,bool>>?filter=null,
        params Expression<Func<Product, object>>[] includeProperties)
    {
        var products = await _db.ProductRepository.GetAllAsync(filter, includeProperties);
        return _map.Map<IEnumerable<ProductDTO>>(products);
    }

    public async Task<ProductDTO> GetProductAsync(int id, params Expression<Func<Product, object>>[]? includeProperties)
    {
        var product = await _db.ProductRepository.GetByIdAsync(id,false,includeProperties);
        if (product==null)
        {
            throw new ProductNotFoundExeption(id);
        }
        
        return _map.Map<ProductDTO>(product);
    }

    public async Task UpdateProductAsync(ProductForUpdateDto productForUpdateDto)
    {
        var validationResult = await _productForUpdateDtoValidator.ValidateAsync(productForUpdateDto);
        if (!validationResult.IsValid)
        {
            throw new NotValidateModelException(nameof(ProductDTO), validationResult.ToString("\n"));
        }
        
        var product = await _db.ProductRepository.GetByIdAsync(productForUpdateDto.Id, true,
            u=>u.ProductCategories,
            u=>u.Images);

        if (product is null)
        {
            throw new ProductNotFoundExeption(productForUpdateDto.Id);
        }
        
        product.Description = productForUpdateDto.Description;
        product.Name = productForUpdateDto.Name;
        product.Translate = productForUpdateDto.Translate;
        product.Year = productForUpdateDto.Year;
        product.TransitTime = productForUpdateDto.TransitTime;
        
        
        if (productForUpdateDto.ImagesId.Length>0)
        {
            var removeImages = product.Images.Where(u => !productForUpdateDto.ImagesId.Contains(u.Id)).ToList();
            _imageOperations.RemoveImages(removeImages.Select(u=>u.ImgName).ToArray());
            foreach (var image in removeImages)
            {
                product.Images.Remove(image);
            }
        }
        
        if (productForUpdateDto.ImageCollection.Count>0)
        {
            await _imageOperations.SaveImagesAsync(productForUpdateDto.ImageCollection.ToArray());
            foreach (var image in productForUpdateDto.ImageCollection)
            {
                product.Images.Add(new Image
                {
                    ImgName = image.FileName,
                    Product = product
                });
            }
        }
        var categories = await _db.ProductCategoryRepository
            .GetAllAsync(u => productForUpdateDto.ProductCategoriesId.Contains(u.Id));
        product.ProductCategories = categories.ToList();
        
        await _db.SaveChangesAsync();
    }

    public async Task RemoveProductAsync(int id)
    {
        var product = await _db.ProductRepository.GetByIdAsync(id,true,u=>u.Images);
        if (product==null)
        {
            throw new ProductNotFoundExeption(id);
        }

        _imageOperations.RemoveImages(product.Images.Select(u=>u.ImgName).ToArray());
        _db.ProductRepository.Remove(product);
        
        await _db.SaveChangesAsync();
    }
}