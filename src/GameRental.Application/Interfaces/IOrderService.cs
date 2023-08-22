using System.Linq.Expressions;
using GameRental.Application.DTO.Account;
using GameRental.Application.DTO.Order;
using GameRental.Domain.Entities;

namespace GameRental.Application.Interfaces;

public interface IOrderService
{
    Task<OrderDto> GetOrderByIdAsync(int id,params Expression<Func<Order, object>>[] includeProperties);
    Task<List<OrderDto>> GetOrdersAsync(Expression<Func<Order,bool>>? filter = null,params Expression<Func<Order, object>>[] includeProperties);
    Task CreateOrderAsync(OrderAddDto orderAddDtoDto);
}