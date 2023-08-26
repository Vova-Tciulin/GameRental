using System.Linq.Expressions;
using GameRental.Domain.Entities;
using GameRental.Domain.IRepository;
using GameRental.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace GameRental.Infrastructure.Repository;

internal class ProductRepository:IProductRepository
{
    private readonly GameRentalDbContext _db;

    public ProductRepository(GameRentalDbContext db)
    {
        _db = db;
    }

    
    public async Task<List<Product>> GetAllAsync(Expression<Func<Product,bool>>?filter=null,params Expression<Func<Product, object>>[] includeProperties)
    {
        IQueryable<Product> query = _db.Products;
        
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

    public async Task<Product?> GetByIdAsync(int id,bool asTracking=false,params Expression<Func<Product, object>>[]? includeProperties)
    {
        QueryTrackingBehavior track = asTracking switch
        {
            true => QueryTrackingBehavior.TrackAll,
            false => QueryTrackingBehavior.NoTracking
        };
        
        IQueryable<Product> query = _db.Products;
        foreach (var includeProperty in includeProperties)
        {
            query=query.Include(includeProperty);
        }
        query=query.AsTracking(track);

        return await query.FirstOrDefaultAsync(u=>u.Id==id);
    }

    public void Add(Product product)
    {
        _db.Products.Add(product);
    }

    public void Update(Product product)
    {
        _db.Products.Update(product);
    }
    public void Remove(Product product)
    {
        _db.Products.Remove(product);
    }
}