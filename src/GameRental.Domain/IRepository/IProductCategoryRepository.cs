using System.Linq.Expressions;
using GameRental.Domain.Entities;

namespace GameRental.Domain.IRepository;

public interface IProductCategoryRepository
{
    Task<IEnumerable<ProductCategory>> GetAllAsync(Expression<Func<ProductCategory,bool>>filter=null);
    Task<ProductCategory> GetByIdAsync(int id);
    void Add(ProductCategory productCategory);
    void Update(ProductCategory productCategory);
    void Remove(ProductCategory productCategory);
}