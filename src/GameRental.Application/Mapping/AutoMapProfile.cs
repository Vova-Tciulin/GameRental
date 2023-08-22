using AutoMapper;
using GameRental.Application.DTO;
using GameRental.Application.DTO.Account;
using GameRental.Application.DTO.Category;
using GameRental.Application.DTO.Console;
using GameRental.Application.DTO.Image;
using GameRental.Application.DTO.Order;
using GameRental.Application.DTO.Product;
using GameRental.Application.DTO.User;
using GameRental.Domain.Entities;
using Console = GameRental.Domain.Entities.Console;

namespace GameRental.Application.Mapping;

public class AutoMapProfile:Profile
{
    public AutoMapProfile()
    {
        CreateMap<Account, AccountDto>();
        CreateMap<AccountDto,Account>();
        CreateMap<AccountForAddDto, Account>();
        CreateMap<AccountForUpdateDto, Account>();
        
        CreateMap<ProductForUpdateDto, Product>();
        CreateMap<ProductAddDto, Product>();
        CreateMap<ProductDTO, Product>();
        CreateMap<Product, ProductDTO>();
        
        CreateMap<ProductCategoryDTO, ProductCategory>();
        CreateMap<ProductCategory, ProductCategoryDTO>();
        CreateMap<ProductCategoryDTO, CategoryForUpsertDto>();
        CreateMap<CategoryForUpsertDto, ProductCategory>();
        
        CreateMap<Image, ImageDTO>();
        CreateMap<ImageDTO, Image>();
        
        CreateMap<Console, ConsoleDto>();
        CreateMap<ConsoleDto, Console>();
        
        CreateMap<UserRegistrationDto, User>();
        CreateMap<User, UserDto>();
        CreateMap<User, UserDetailsDto>();
        
        CreateMap<Order, OrderDto>();
        CreateMap<OrderAddDto, Order>();
        
    }
}