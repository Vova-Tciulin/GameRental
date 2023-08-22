using GameRental.Application.DTO.Image;
using Microsoft.AspNetCore.Http;


namespace GameRental.Application.DTO.Product;

public class ProductForUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int TransitTime { get; set; }
    public string Translate { get; set; }
    public int Year { get; set; }
    
    public int[] ProductCategoriesId { get; set; }
    public int[] ImagesId { get; set; }
    public List<ImageDTO> Images { get; set; } = new();
    public List<IFormFile> ImageCollection { get; set; } = new();
}