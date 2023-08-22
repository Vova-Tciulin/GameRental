using System.Linq.Expressions;
using GameRental.Domain.Entities;

namespace GameRental.Domain.IRepository;

public interface IOrderRepository
{
    Task<Order?> FirstOrDefaultAsync(Expression<Func<Order, bool>> filter, bool asTracking = false);
     Task<Order?> GetByIdAsync(int id,bool asTracking=false,params Expression<Func<Order, object>>[] includeProperties);
    Task<List<Order>> GetOrdersAsync(Expression<Func<Order,bool>>? filter = null,params Expression<Func<Order, object>>[] includeProperties);
    void Add(Order order);
    
}