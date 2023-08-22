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
using GameRental.Web.Models;
using GameRental.Web.Models.AccountModels;
using GameRental.Web.Models.Cart;
using GameRental.Web.Models.Order;
using GameRental.Web.Models.ProductModels;
using GameRental.Web.Models.User;

namespace GameRental.Web.Mapping;

public class AutoMapProfile:Profile
{
    public AutoMapProfile()
    {
        CreateMap<ImageDTO, ImageVm>();
        CreateMap<ProductForUpdateVm, ProductForUpdateDto>();
        CreateMap<ProductDTO, ProductForUpdateVm>();
        CreateMap<ProductAddVM, ProductAddDto>();
        CreateMap<ProductVM, ProductDTO>();
        CreateMap<ProductDTO, ProductVM>();
        CreateMap<ProductCategoryVM, ProductCategoryDTO>();
        CreateMap<ProductCategoryDTO, ProductCategoryVM>();
        CreateMap<ProductAddVM, ProductDTO>();
        CreateMap<ProductDTO,ProductAddVM>();
        CreateMap<AccountDto, AccountVm>();
        CreateMap<AccountVm, AccountDto>();
        CreateMap<ConsoleDto, ConsoleVm>();
        CreateMap<ConsoleVm,ConsoleDto>();
        CreateMap<AccountForAddVm, AccountForAddDto>();
        CreateMap<AccountDto, AccountForUpdateVm>();
        CreateMap<AccountForUpdateVm, AccountForUpdateDto>();
        CreateMap<UserLoginModel, UserLoginDto>();
        CreateMap<UserRegistrationModel, UserRegistrationDto>();
        CreateMap<UserDto, UserVm>();
        CreateMap<UserDetailsDto, UserDetailsVm>();
        CreateMap<UserDetailsDto, UserChangeRoleVm>()
            .ForMember(dest => dest.AllRoles,
                opt => opt.MapFrom(src => src.Roles));
        CreateMap<UserChangeRoleVm, UserChangeRoleDto>();
        CreateMap<ProductDTO, ProductDetailsVm>();
        CreateMap<AccountDto, AccountInfoVm>();
        CreateMap<AccountDto, CartVm>()
            .ForMember(dest => dest.AccountId,
                opt => opt.MapFrom(src => src.Id));
        CreateMap<CategoryForUpsertVm, CategoryForUpsertDto>();
        CreateMap<ProductCategoryDTO, CategoryForUpsertVm>();
        CreateMap<OrderDto, OrderVm>();
    }
}