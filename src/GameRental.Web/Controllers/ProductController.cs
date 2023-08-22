using System.Linq.Expressions;
using AutoMapper;
using GameRental.Application.DTO;
using GameRental.Application.DTO.Product;
using GameRental.Application.Interfaces;
using GameRental.Domain.Entities;
using GameRental.Infrastructure.Repository;
using GameRental.Web.Models;
using GameRental.Web.Models.AccountModels;
using GameRental.Web.Models.ProductModels;
using Microsoft.AspNetCore.Mvc;

namespace GameRental.Web.Controllers;

public class ProductController:Controller
{
    private readonly IMapper _map;
    private readonly IServiceManager _services;
    public ProductController(IMapper map, IServiceManager services)
    {
        _map = map;
        _services = services;
    }

    public async Task<IActionResult> Index(int? id)
    {
        Expression<Func<Product, bool>>? filter = null;
        if (id != null)
        {
            filter = u => u.ProductCategories.Any(c => c.Id == id);
        }

        var productsDto = await _services.ProductService.GetProductsAsync(filter,
                u=>u.ProductCategories,
                u=>u.Images.Take(1),
                u=>u.Accounts.OrderBy(x=>x.Price).Take(1));
       
        var categoryDto = await _services.ProductCategoryService.GetCategoriesAsync();
        var productAndCategoryVm = new ProductAndCategoryVm()
        {
            CategoryVm=_map.Map<List<ProductCategoryVM>>(categoryDto),
            ProductVm = _map.Map<List<ProductVM>>(productsDto)
        };
        if (id!=null)
        {
            productAndCategoryVm.Category = productAndCategoryVm.CategoryVm.FirstOrDefault(u => u.Id == id);
        }
        
        return View(productAndCategoryVm);
    }

    public async Task<IActionResult> ProductDetails(int id)
    {
        var productDto = await _services.ProductService.GetProductAsync(id,
            u=>u.ProductCategories,
            u=>u.Accounts.OrderBy(x=>x.IsRented),
            u=>u.Images);
        var productDetails = _map.Map<ProductDetailsVm>(productDto);
        return View(productDetails);
    }
}