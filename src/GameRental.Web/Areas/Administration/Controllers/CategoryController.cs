using AutoMapper;
using GameRental.Application.DTO.Category;
using GameRental.Application.DTO.Product;
using GameRental.Application.Interfaces;
using GameRental.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameRental.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = "Admin")]
public class CategoryController:Controller
{
    private readonly IMapper _map;
    private readonly IServiceManager _services;
    
    
    public CategoryController(IMapper map, IServiceManager services)
    {
        _map = map;
        _services = services;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _services.ProductCategoryService.GetCategoriesAsync();
        var caregoriesVm = _map.Map<List<ProductCategoryVM>>(categories);
        return View(caregoriesVm);
        
    }
    
    [HttpGet]
    public IActionResult AddCategory()
    {
        CategoryForUpsertVm categoryForUpsertVm = new CategoryForUpsertVm();
        return View(categoryForUpsertVm);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(CategoryForUpsertVm categoryVm)
    {
        if (ModelState.IsValid&&categoryVm.Image!=null)
        {
            var categoryDto = _map.Map<CategoryForUpsertDto>(categoryVm);
            await _services.ProductCategoryService.AddCategoryAsync(categoryDto);
            return RedirectToAction("Index");
        }
        return View(categoryVm);
    }
    
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var categoryDto = await _services.ProductCategoryService.GetCategoryByIdAsync(id);
        var categoryVm = _map.Map<CategoryForUpsertVm>(categoryDto);
        return View(categoryVm);
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(CategoryForUpsertVm categoryVm)
    {
        if (ModelState.IsValid)
        {
            var categoryDto = _map.Map<CategoryForUpsertDto>(categoryVm);
            await _services.ProductCategoryService.UpdateProductCategoryAsync(categoryDto);
            return RedirectToAction("Index");
        }

        return View(categoryVm);
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        await _services.ProductCategoryService.RemoveProductCategoryAsync(id);
        
        return Ok();
    }
}