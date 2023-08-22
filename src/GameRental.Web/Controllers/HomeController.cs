using System.Diagnostics;
using AutoMapper;
using GameRental.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GameRental.Web.Models;
using GameRental.Web.Models.ProductModels;

namespace GameRental.Web.Controllers;

public class HomeController : Controller
{
    private readonly IMapper _map;
    private readonly IServiceManager _services;

    public HomeController(IMapper map, IServiceManager services)
    {
        _map = map;
        _services = services;
    }

    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}