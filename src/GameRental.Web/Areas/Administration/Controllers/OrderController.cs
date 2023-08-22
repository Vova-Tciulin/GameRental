using AutoMapper;
using GameRental.Application.Interfaces;
using GameRental.Web.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameRental.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = "Admin")]
public class OrderController:Controller
{
    private readonly IMapper _map;
    private readonly IServiceManager _services;

    public OrderController(IMapper map, IServiceManager services)
    {
        _map = map;
        _services = services;
    }

    public async Task<IActionResult> Index()
    {
        var ordersDto=await _services.OrderService.GetOrdersAsync(null,
            u=>u.Account,
            u=>u.User);
        var ordersVm = _map.Map<List<OrderVm>>(ordersDto);

        return View(ordersVm);
    }
}