using System.Linq.Expressions;
using GameRental.Domain.Entities;
using GameRental.Domain.IRepository;
using GameRental.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace GameRental.Infrastructure.Repository;

public class OrderRepository:IOrderRepository
{
    private readonly GameRentalDbContext _db;

    public OrderRepository(GameRentalDbContext db)
    {
        _db = db;
    }

    public async Task<Order?> FirstOrDefaultAsync(Expression<Func<Order, bool>> filter, bool asTracking = false)
    {
        QueryTrackingBehavior track = asTracking switch
        {
            true => QueryTrackingBehavior.TrackAll,
            false => QueryTrackingBehavior.NoTracking
        };

        return await _db.Orders.AsTracking(track).FirstOrDefaultAsync(filter);
    }

    public async Task<Order?> GetByIdAsync(int id, bool asTracking = false,params Expression<Func<Order, object>>[] includeProperties)
    {
        QueryTrackingBehavior track = asTracking switch
        {
            true => QueryTrackingBehavior.TrackAll,
            false => QueryTrackingBehavior.NoTracking
        };
        
        IQueryable<Order> query = _db.Orders;
        foreach (var includeProperty in includeProperties)
        {
            query=query.Include(includeProperty);
        }
        query=query.AsTracking(track);

        return await query.FirstOrDefaultAsync(u=>u.Id==id);
    }

    public async Task<List<Order>> GetOrdersAsync(Expression<Func<Order, bool>>? filter = null,params Expression<Func<Order, object>>[] includeProperties)
    {
        IQueryable<Order> query = _db.Orders;
        
        if (filter!=null)
        {
            query=query.Where(filter);
        }
        
        foreach (var includeProperty in includeProperties)
        {
            query=query.Include(includeProperty);
        }

        return await query.ToListAsync();
    }

    public void Add(Order order)
    {
        _db.Orders.Add(order);
    }

    
}