using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using GameRental.Application.DTO.Order;
using GameRental.Application.Exceptions;
using GameRental.Application.Interfaces;
using GameRental.Domain.Entities;
using GameRental.Domain.Exceptions;
using GameRental.Domain.IRepository;
using GameRental.Infrastructure.Email;
using GameRental.Infrastructure.Email.Intefaces;
using Microsoft.AspNetCore.Identity;

namespace GameRental.Application.Services;

public class OrderService:IOrderService
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _map;
    private readonly IValidator<OrderAddDto> _orderForAddValidator;
    private readonly UserManager<User> _userManager;
    private readonly IEmailSender _emailSender;

    public OrderService(IUnitOfWork db, IMapper map, IValidator<OrderAddDto> orderForAddValidator, UserManager<User> userManager, IEmailSender emailSender)
    {
        _db = db;
        _map = map;
        _orderForAddValidator = orderForAddValidator;
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public async Task<OrderDto> GetOrderByIdAsync(int id,params Expression<Func<Order, object>>[] includeProperties)
    {
        var order = await _db.OrderRepository.GetByIdAsync(id,false,includeProperties);

        if (order==null)
        {
            throw new OrderNotFoundException(id);
        }
        var orderDto = _map.Map<OrderDto>(order);
        
        return orderDto;
    }

    public async Task<List<OrderDto>> GetOrdersAsync(Expression<Func<Order, bool>>? filter = null,params Expression<Func<Order, object>>[] includeProperties)
    {
        var orders = await _db.OrderRepository.GetOrdersAsync(filter,includeProperties);
        var ordersDto = _map.Map<List<OrderDto>>(orders);
        return ordersDto;
    }

    public async Task CreateOrderAsync(OrderAddDto orderAddDto)
    {
        var validationResult = await _orderForAddValidator.ValidateAsync(orderAddDto);
        if (!validationResult.IsValid)
        {
            throw new NotValidateModelException(nameof(OrderAddDto), validationResult.ToString("\n"));
        }

        var account = await _db.AccountRepository.GetByIdAsync(orderAddDto.AccountId,true);
        if (account==null)
        {
            throw new AccountNotFoundException(orderAddDto.AccountId);
        }
        
        var order = _map.Map<Order>(orderAddDto);

        var user = await _userManager.FindByEmailAsync(orderAddDto.UserEmail);
        if (user==null)
        {
            throw new UserNotFoundException(orderAddDto.UserEmail);
        }

        order.IsActive = true;
        order.UserId = user.Id;
        order.OrderDate=DateTime.Today;
        order.EndOrderDate = order.OrderDate.AddDays(orderAddDto.DayOfRent);
        account.IsRented = true;
        
        _db.OrderRepository.Add(order);
        await _db.SaveChangesAsync();
        await SendMessageWithAccount(account, user.Email, order.EndOrderDate);
        
    }

    private async Task SendMessageWithAccount(Account account,string email,DateTime date)
    {
        string content = $"Спасибо за аренду аккаунта {account.Name}\n" +
                         $"Логин: {account.Login}\nПароль: {account.Password}\n" +
                         $"Аренда аккаунта до {date}.";

        var message = new Message(new string[]{email}, "Данные от аккаунта", content);
        await _emailSender.SendEmailAsync(message);
    }
}