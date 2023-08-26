using System.Security.Claims;
using System.Security.Policy;
using AutoMapper;
using FluentValidation;
using GameRental.Application.DTO.Product;
using GameRental.Application.DTO.User;
using GameRental.Application.Interfaces;
using GameRental.Application.SomeLogic;
using GameRental.Domain.Entities;
using GameRental.Domain.Exceptions;
using GameRental.Domain.IRepository;
using GameRental.Infrastructure.EF;
using GameRental.Infrastructure.Email;
using GameRental.Infrastructure.Email.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace GameRental.Application.Services;

public class IdentityService:IIdentityService
{

    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _map;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityService(
        IMapper map,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _map = map;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public async Task<OperationDetails> CreateUserAsync(UserRegistrationDto userDto)
    {
        
        var user = _map.Map<User>(userDto);
        user.UserName = userDto.Email;

        var result = await _userManager.CreateAsync(user,userDto.Password);
        if (!result.Succeeded)
        {
            return new OperationDetails(false,result.Errors.ToString(),"");
        }

        var numOfUsers=await _userManager.Users.CountAsync();
        if (numOfUsers==1)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }
        await _userManager.AddToRoleAsync(user, "User");
        
        return new OperationDetails(true,"Регистрация прошла успешно","");
    }

    public async Task<OperationDetails> AuthenticateAsync(UserLoginDto user)
    {
        var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);
        if (result.Succeeded)
        {
            return new OperationDetails(true, "Вход выполнен успешно", "");
        }
        
        return new OperationDetails(false, "Неправильный логин или пароль", "");
        
    }

    public async Task LogOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<string> GetTokenForChangePasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return string.Empty;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return token;
    }

    public async Task<OperationDetails> ChangePasswordAsync(string email, string newPassword, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user==null)
        {
            return new OperationDetails(false, "User not found", "");
        }

        var resetPassResult = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (!resetPassResult.Succeeded)
        {
            return new OperationDetails(false, resetPassResult.Errors.ToString(), "");
        }

        return new OperationDetails(true, "succeeded", "");
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var usersDto = _map.Map<List<UserDto>>(users);
        return usersDto;
    }

    public async Task<UserDetailsDto> GetUserDetailsAsync(string id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user==null)
        {
            throw new UserNotFoundException(id);
        }
        
        var roles = await _userManager.GetRolesAsync(user);
        var userDetailsDto = _map.Map<UserDetailsDto>(user);

        userDetailsDto.Roles = roles.ToList();

        return userDetailsDto;
    }

    public async Task<List<string>> GetAllRolesAsync()
    {
        var roles=await _roleManager.Roles.Select(u=>u.Name).ToListAsync();

        return roles;
    }

    public async Task ChangeRoleAsync(UserChangeRoleDto userChangeRoleDto)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userChangeRoleDto.Id);
        if (user==null)
        {
            throw new UserNotFoundException(userChangeRoleDto.Id);
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, userRoles);

        if (result.Succeeded)
        {
            await _userManager.AddToRolesAsync(user, userChangeRoleDto.NewRoles);
        }
    }
}