using System.Security.Claims;
using AutoMapper;
using GameRental.Application.DTO.User;
using GameRental.Application.Interfaces;
using GameRental.Domain.Entities;
using GameRental.Infrastructure.Email;
using GameRental.Infrastructure.Email.Intefaces;
using GameRental.Web.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GameRental.Web.Controllers;

public class AccountController:Controller
{

    private readonly IMapper _map;
    private readonly IServiceManager _services;
    private readonly IEmailSender _emailSender;
    
    public AccountController(IMapper map, IServiceManager services, IEmailSender emailSender)
    {
        _map = map;
        _services = services;
        _emailSender = emailSender;
    }

    
    [HttpGet]
    public IActionResult Register()
    {
       
        var userRegistrationModel = new UserRegistrationModel();
        
        return View(userRegistrationModel);
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegistrationModel userRegistrationModel)
    {
        
        
        if (!ModelState.IsValid)
        {
            return View(userRegistrationModel);
        }

        var userDto = _map.Map<UserRegistrationDto>(userRegistrationModel);

        var result = await _services.IdentityService.CreateUserAsync(userDto);

        if (!result.Succedeed)
        {
            ModelState.AddModelError("",result.Message);
            return View(userRegistrationModel);
        }
        
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult Login()
    {
        var loginModel = new UserLoginModel();
        return View(loginModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserLoginModel userLoginModel)
    {
        if (!ModelState.IsValid)
        {
            return View(userLoginModel);
        }

        var userDto = _map.Map<UserLoginDto>(userLoginModel);
        var result = await _services.IdentityService.AuthenticateAsync(userDto);
        
        if (result.Succedeed)
        {
            
            return RedirectToAction("Index","Product");
        }
        
        ModelState.AddModelError("", result.Message);
        
        return View();
    }
    
    
    
    public async Task<IActionResult> Logout()
    {
        await _services.IdentityService.LogOutAsync();
        return RedirectToAction("Index", "Product");
    }
    
    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPassworddModel forgotPasswordModel)
    {
        if (!ModelState.IsValid)
        {
            return View(forgotPasswordModel);
        }

        var token = await _services.IdentityService.GetTokenForChangePasswordAsync(forgotPasswordModel.Email);
        
        if (token.IsNullOrEmpty())
        {
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        var callback = Url.Action("ResetPassword", "Account", new { token, email = forgotPasswordModel.Email },
            Request.Scheme);

        var message = new Message(new string[] { forgotPasswordModel.Email }, "Reset password", callback);
        await _emailSender.SendEmailAsync(message);
        
        return RedirectToAction("ForgotPasswordConfirmation");

    }
    
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(string token,string email)
    {
        var model = new ResetPasswordModel { Token = token, Email = email };
        
        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
    {
        if (!ModelState.IsValid)
        {
            return View(resetPasswordModel);
        }

        var res = await _services.IdentityService.ChangePasswordAsync(resetPasswordModel.Email,
            resetPasswordModel.Password, resetPasswordModel.Token);

        return RedirectToAction("ResetPasswordConfirmation");
    }
    
    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }
}