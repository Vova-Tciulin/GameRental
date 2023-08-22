using AutoMapper;
using GameRental.Application.DTO.Order;
using GameRental.Application.Interfaces;
using GameRental.Web.Extensions;
using GameRental.Web.Models.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameRental.Web.Controllers;

public class CartController:Controller
{
    private readonly IMapper _map;
    private readonly IServiceManager _services;

    public CartController(IMapper map, IServiceManager services)
    {
        _map = map;
        _services = services;
    }
    
    public async Task<IActionResult> AddToCart(int id)
    {
        if (HttpContext.Session.Keys.Contains("product"))
        {
            return BadRequest("В корзине уже добавлен товар");
        }
        
        var accountDto = await _services.AccountService.GetByIdAsync(id);
        var cartVm = _map.Map<CartVm>(accountDto);
        var productDto = await _services.ProductService.GetProductAsync(cartVm.ProductId,u=>u.Images.Take(1));
        cartVm.DayOfRent = 7;
        cartVm.ImgName = productDto.Images[0].ImgName;
        
        HttpContext.Session.Set<CartVm>("product",cartVm);
        HttpContext.Session.Set<int>("accountId",id);
        
        return Ok();
    }

    

    public IActionResult ViewCart()
    {
        var cart = HttpContext.Session.Get<CartVm>("product");
        
        return View(cart);
    }
    
    [Authorize]
    public async Task<IActionResult> MakeOrder(int days)
    {
        var cart = HttpContext.Session.Get<CartVm>("product");
        if (cart==null)
        {
            return View("ViewCart");
        }
        
        var orderForAddDto = new OrderAddDto()
        {
            UserEmail = User.Identity.Name,
            AccountId = cart.AccountId,
            Cost = cart.Price * cart.DayOfRent / 7,
            DayOfRent = cart.DayOfRent
        };
        HttpContext.Session.Remove("product");
        HttpContext.Session.Remove("accountId");
        
        await _services.OrderService.CreateOrderAsync(orderForAddDto);
        
        return RedirectToAction("Info","Cart");
    }

    public IActionResult DeleteProductFromCart()
    {
        if (!HttpContext.Session.Keys.Contains("product") || !HttpContext.Session.Keys.Contains("accountId"))
            return BadRequest("В корзине нет товаров");
        
        HttpContext.Session.Remove("product");
        HttpContext.Session.Remove("accountId");
        return Ok();

    }

    public IActionResult Info()
    {
        return View();
    }
}