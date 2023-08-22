using GameRental.Application.DTO;
using GameRental.Application.DTO.Category;
using GameRental.Application.DTO.Product;

namespace GameRental.Application.Interfaces;

public interface IProductCategoryService
{
    Task<ProductCategoryDTO> GetCategoryByIdAsync(int id);
    Task AddCategoryAsync(CategoryForUpsertDto productCategoryDto);
    Task<IEnumerable<ProductCategoryDTO>> GetCategoriesAsync();
    Task UpdateProductCategoryAsync(CategoryForUpsertDto productCategoryDto);
    Task RemoveProductCategoryAsync(int id);
}