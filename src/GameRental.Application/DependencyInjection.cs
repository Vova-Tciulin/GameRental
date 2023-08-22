using AutoMapper;
using FluentValidation;
using GameRental.Application.DTO;
using GameRental.Application.DTO.Account;
using GameRental.Application.DTO.Category;
using GameRental.Application.DTO.Order;
using GameRental.Application.DTO.Product;
using GameRental.Application.Interfaces;
using GameRental.Application.Mapping;
using GameRental.Application.Services;
using GameRental.Application.SomeLogic;
using GameRental.Application.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace GameRental.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services,string webRootPath)
    {
        services.AddAutoMapper(typeof(AutoMapProfile));
        services.AddScoped<IServiceManager, ServiceManager>();
        services.AddScoped<IValidator<ProductAddDto>, ProductAddDtoValidator>();
        services.AddScoped<IValidator<ProductForUpdateDto>, ProductForUpdateDtoValidator>();
        services.AddScoped<IValidator<AccountForAddDto>, AccountForAddDtoValidator>();
        services.AddScoped<IValidator<AccountForUpdateDto>, AccountForUpdateDtoValidator>();
        services.AddScoped<IValidator<OrderAddDto>, OrderForAddDtoValidator>();
        services.AddSingleton<IImageOperations>(u=>new ImageOperations(webRootPath));
        return services;
    }
}