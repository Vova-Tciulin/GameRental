using System.Linq.Expressions;
using GameRental.Domain.Entities;

namespace GameRental.Domain.IRepository;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(Expression<Func<Product,bool>>? filter = null, params Expression<Func<Product, object>>[] includeProperties);
    Task<Product?> GetByIdAsync(int id,bool asTracking=false,params Expression<Func<Product, object>>[] includeProperties);
    void Add(Product product);
    void Update(Product product);
    void Remove(Product product);
}