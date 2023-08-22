using GameRental.Application.DTO.Account;
using GameRental.Application.DTO.Category;
using GameRental.Application.DTO.Console;
using GameRental.Application.DTO.Image;

namespace GameRental.Application.DTO.Product;

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int TransitTime { get; set; }
    public string Translate { get; set; }
    public int Year { get; set; }
    public List<AccountDto> Accounts { get; set; } = new();
    public List<ProductCategoryDTO> ProductCategories { get; set; } = new();
    public List<ImageDTO> Images { get; set; } = new();
}