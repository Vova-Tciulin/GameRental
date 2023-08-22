using System.Linq.Expressions;
using GameRental.Application.DTO;
using GameRental.Application.DTO.Product;
using GameRental.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace GameRental.Application.Interfaces;

public interface IProductService
{
    Task AddProductAsync(ProductAddDto productAddDto);
    Task<IEnumerable<ProductDTO>> GetProductsAsync(Expression<Func<Product,bool>>?filter=null,params Expression<Func<Product, object>>[] includeProperties);
    Task<ProductDTO> GetProductAsync(int id,params Expression<Func<Product, object>>[] includeProperties);
    Task UpdateProductAsync(ProductForUpdateDto productForUpdateDto);
    Task RemoveProductAsync(int id);
}