using AutoMapper;
using GameRental.Application.DTO.User;
using GameRental.Application.Interfaces;
using GameRental.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GameRental.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = "Admin")]
public class UserController:Controller
{
    private readonly IMapper _map;
    private readonly IServiceManager _services;

    public UserController(IMapper map, IServiceManager services)
    {
        _map = map;
        _services = services;
    }

    public async Task<IActionResult> Index()
    {
        var usersDto = await _services.IdentityService.GetAllUsersAsync();
        var usersVm = _map.Map<List<UserVm>>(usersDto);

        return View(usersVm);
    }

    [HttpGet]
    public async Task<IActionResult> UserDetails(string id)
    {
        var userDetailsDto = await _services.IdentityService.GetUserDetailsAsync(id);
        var userDetailsVm = _map.Map<UserDetailsVm>(userDetailsDto);
        
        return View(userDetailsVm);
    }

    [HttpGet]
    public async Task<IActionResult> ChangeRole(string id)
    {
        var userDetailsDto = await _services.IdentityService.GetUserDetailsAsync(id);
        var userForEditVm = _map.Map<UserChangeRoleVm>(userDetailsDto);
        userForEditVm.NewRoles = userDetailsDto.Roles.ToArray();
        userForEditVm.AllRoles = await _services.IdentityService.GetAllRolesAsync();
        
        return View(userForEditVm);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeRole(UserChangeRoleVm userChangeRoleVm)
    {
        if (!ModelState.IsValid)
        {
            userChangeRoleVm.AllRoles = await _services.IdentityService.GetAllRolesAsync();
            return View(userChangeRoleVm);
        }

        var userChangeRoleDto = _map.Map<UserChangeRoleDto>(userChangeRoleVm);
        await _services.IdentityService.ChangeRoleAsync(userChangeRoleDto);

        return RedirectToAction("Index");
    }

}