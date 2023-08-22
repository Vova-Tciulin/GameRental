using AutoMapper;
using GameRental.Application.DTO;
using GameRental.Application.DTO.Product;
using GameRental.Application.Interfaces;
using GameRental.Domain.Entities;
using GameRental.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameRental.Web.Controllers;

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
        var categoriesDto = await _services.ProductCategoryService.GetCategoriesAsync();
        var categoriesVm = _map.Map<IEnumerable<ProductCategoryVM>>(categoriesDto);

        return View(categoriesVm);
    }
    
}