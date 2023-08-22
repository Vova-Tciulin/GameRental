using AutoMapper;
using GameRental.Application.DTO.Product;
using GameRental.Application.Interfaces;
using GameRental.Web.Models;
using GameRental.Web.Models.ProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameRental.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = "Admin")]
public class ProductController:Controller
{
    private readonly IMapper _map;
    private readonly IServiceManager _services;
    public ProductController(IMapper map, IServiceManager services)
    {
        _map = map;
        _services = services;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _services.ProductService.GetProductsAsync(null,u=>u.ProductCategories);
        var productsVM = _map.Map<IEnumerable<ProductVM>>(products);

        return View(productsVM);
    }
    
    [HttpGet]
    public async Task<IActionResult> AddProduct()
    {
        var productVM = new ProductAddVM();
        var categories = await _services.ProductCategoryService.GetCategoriesAsync();
        productVM.Categories = _map.Map<List<ProductCategoryVM>>(categories);
        
        return View(productVM);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductAddVM product)
    {
        if (ModelState.IsValid)
        {
            var productAddDto = _map.Map<ProductAddDto>(product);
            await _services.ProductService.AddProductAsync(productAddDto);
            return RedirectToAction("Index");
        }
        var categories = await _services.ProductCategoryService.GetCategoriesAsync();
        product.Categories = _map.Map<List<ProductCategoryVM>>(categories);
        return View(product);
    }
    
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var productDto = await _services.ProductService.GetProductAsync(id,
            u=>u.ProductCategories,
            u=>u.Images);
        var productForUpdate = _map.Map<ProductForUpdateVm>(productDto);
        var categoriesDto = await _services.ProductCategoryService.GetCategoriesAsync();
        
        productForUpdate.AllCategories = _map.Map<List<ProductCategoryVM>>(categoriesDto);
        productForUpdate.ProductCategoriesId = productDto.ProductCategories.Select(u=>u.Id).ToArray();
        
        return View(productForUpdate);
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(ProductForUpdateVm productForUpdate)
    {
        if (ModelState.IsValid)
        {
            var productForUpdateDto = _map.Map<ProductForUpdateDto>(productForUpdate);
            await _services.ProductService.UpdateProductAsync(productForUpdateDto);
            return RedirectToAction("Index");
        }
        
        var categories = await _services.ProductCategoryService.GetCategoriesAsync();
        productForUpdate.AllCategories = _map.Map<List<ProductCategoryVM>>(categories);
        
        return View(productForUpdate);
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        await _services.ProductService.RemoveProductAsync(id);
        return Ok();
    }
    
}