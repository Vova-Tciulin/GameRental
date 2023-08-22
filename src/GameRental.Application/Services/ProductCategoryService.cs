using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using GameRental.Application.DTO;
using GameRental.Application.DTO.Category;
using GameRental.Application.DTO.Product;
using GameRental.Application.Exceptions;
using GameRental.Application.Interfaces;
using GameRental.Application.SomeLogic;
using GameRental.Application.Validation;
using GameRental.Domain.Entities;
using GameRental.Domain.Exceptions;
using GameRental.Domain.IRepository;
using Microsoft.AspNetCore.Http;

namespace GameRental.Application.Services;

public class ProductCategoryService:IProductCategoryService
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _map;
    private readonly IImageOperations _imageOperations;

    public ProductCategoryService(IUnitOfWork db, IMapper map, IImageOperations imageOperations)
    {
        _db = db;
        _map = map;
        _imageOperations = imageOperations;
    }

    public async Task<ProductCategoryDTO> GetCategoryByIdAsync(int id)
    {
        var category = await _db.ProductCategoryRepository.GetByIdAsync(id);
        if (category==null)
        {
            throw new CategoryNotFoundException(id);
        }
        return _map.Map<ProductCategoryDTO>(category);
    }

    public async Task AddCategoryAsync(CategoryForUpsertDto categoryDto)
    {
        var productCategory = _map.Map<ProductCategory>(categoryDto);
        
        await _imageOperations.SaveImagesAsync(categoryDto.Image);
        productCategory.ImgPath =categoryDto.Image.FileName; 
        
        _db.ProductCategoryRepository.Add(productCategory);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductCategoryDTO>> GetCategoriesAsync()
    {
        var categories = await _db.ProductCategoryRepository.GetAllAsync();
        return _map.Map<IEnumerable<ProductCategoryDTO>>(categories);
    }
    
    public async Task UpdateProductCategoryAsync(CategoryForUpsertDto productCategoryDto)
    {

        var category = await _db.ProductCategoryRepository.GetByIdAsync(productCategoryDto.Id);
        
        if (category==null)
        {
            throw new CategoryNotFoundException(productCategoryDto.Id);
        } 
        
        category.Name = productCategoryDto.Name;
        
        if (productCategoryDto.Image!=null)
        {
            
            _imageOperations.RemoveImages(category.ImgPath);
            await _imageOperations.SaveImagesAsync(productCategoryDto.Image);
            
            category.ImgPath = productCategoryDto.Image.FileName;
        }
        
        _db.ProductCategoryRepository.Update(category);
        await _db.SaveChangesAsync();
    }

    public async Task RemoveProductCategoryAsync(int id)
    {
        var category = await _db.ProductCategoryRepository.GetByIdAsync(id);
        
        if (category==null)
        {
            throw new CategoryNotFoundException(id);
        } 
        
        _imageOperations.RemoveImages(category.ImgPath);
        
        _db.ProductCategoryRepository.Remove(category); 
        await _db.SaveChangesAsync();
    }

    
}