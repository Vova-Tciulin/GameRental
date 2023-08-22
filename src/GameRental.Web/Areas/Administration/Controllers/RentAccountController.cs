using AutoMapper;
using GameRental.Application.DTO.Account;
using GameRental.Application.Interfaces;
using GameRental.Web.Models;
using GameRental.Web.Models.AccountModels;
using GameRental.Web.Models.ProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameRental.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = "Admin")]
public class RentAccountController:Controller
{
    private readonly IMapper _map;
    private readonly IServiceManager _services;

    public RentAccountController(IMapper map, IServiceManager services)
    {
        _map = map;
        _services = services;
    }
    
    public async Task<IActionResult> Index()
    {
        var accountsDto =await _services.AccountService.GetAllAsync(null,
            u=>u.Consoles,
            u=>u.Orders.Where(x=>x.IsActive==true));
        var accounts = _map.Map<List<AccountVm>>(accountsDto);
        return View(accounts);
    }

    [HttpGet]
    public async Task<IActionResult> AddAccount()
    {
        var accountForAdd = new AccountForAddVm();
        var productsDto= await _services.ProductService.GetProductsAsync();
        var consolesDto = await _services.ConsoleService.GetAllAsync();
        
        accountForAdd.Products = _map.Map<List<ProductVM>>(productsDto);
        accountForAdd.Consoles = _map.Map<List<ConsoleVm>>(consolesDto);

        return View(accountForAdd);
    }

    [HttpPost]
    public async Task<IActionResult> AddAccount(AccountForAddVm accountForAddVm)
    {
        if (ModelState.IsValid)
        {
            var accountForAddDto = _map.Map<AccountForAddDto>(accountForAddVm);

            await _services.AccountService.AddAccountAsync(accountForAddDto);
            return RedirectToAction("Index");
        }
      
        var productsDto= await _services.ProductService.GetProductsAsync();
        var consolesDto = await _services.ConsoleService.GetAllAsync();
    
        accountForAddVm.Products = _map.Map<List<ProductVM>>(productsDto);
        accountForAddVm.Consoles = _map.Map<List<ConsoleVm>>(consolesDto);

        return View(accountForAddVm);
        
    }
    
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var account = await _services.AccountService.GetByIdAsync(id,u=>u.Consoles);
        var productsDto= await _services.ProductService.GetProductsAsync();
        var consolesDto = await _services.ConsoleService.GetAllAsync();
        
        var accountForUpdateVm = _map.Map<AccountForUpdateVm>(account);
        
        accountForUpdateVm.SelectedConsoles = account.Consoles.Select(u => u.Id).ToArray();
        accountForUpdateVm.Products = _map.Map<List<ProductVM>>(productsDto);
        accountForUpdateVm.Consoles = _map.Map<List<ConsoleVm>>(consolesDto);

        return View(accountForUpdateVm);

    }

    [HttpPost]
    public async Task<IActionResult> Update(AccountForUpdateVm accountForUpdateVm)
    {
        if (ModelState.IsValid)
        {
            var accountForUpdateDto = _map.Map<AccountForUpdateDto>(accountForUpdateVm);
            await _services.AccountService.UpdateAsync(accountForUpdateDto);

            return RedirectToAction("Index");
        }
        
        var productsDto= await _services.ProductService.GetProductsAsync();
        var consolesDto = await _services.ConsoleService.GetAllAsync();
            
        accountForUpdateVm.Products = _map.Map<List<ProductVM>>(productsDto);
        accountForUpdateVm.Consoles = _map.Map<List<ConsoleVm>>(consolesDto);
            
        return View(accountForUpdateVm);
        
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        await _services.AccountService.RemoveAsync(id);
        return Ok();
    }
}