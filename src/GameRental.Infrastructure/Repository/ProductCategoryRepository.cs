using System.Linq.Expressions;
using GameRental.Domain.Entities;
using GameRental.Domain.IRepository;
using GameRental.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace GameRental.Infrastructure.Repository;

internal class ProductCategoryRepository:IProductCategoryRepository
{
    private readonly GameRentalDbContext _db;

    public ProductCategoryRepository(GameRentalDbContext db)
    {
        _db = db;
    }
    public async Task<IEnumerable<ProductCategory>> GetAllAsync(Expression<Func<ProductCategory, bool>> filter = null)
    {
        if (filter!= null)
        {
            return await _db.ProductCategories.Where(filter).ToListAsync();
        }
        return await _db.ProductCategories.ToListAsync();
    }

    public async Task<ProductCategory> GetByIdAsync(int id)
    {
        return await _db.ProductCategories.FindAsync(id);
    }


    public void Add(ProductCategory productCategory)
    {
        _db.ProductCategories.Add(productCategory);
    }

    public void Update(ProductCategory productCategory)
    {
        _db.ProductCategories.Update(productCategory);
    }
    
    public void Remove(ProductCategory productCategory)
    {
       _db.ProductCategories.Remove(productCategory);
    }
}