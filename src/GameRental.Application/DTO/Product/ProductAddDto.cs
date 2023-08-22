using Microsoft.AspNetCore.Http;



namespace GameRental.Application.DTO.Product;

public class ProductAddDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int[] ProductCategoriesId { get; set; }
    public int TransitTime { get; set; }
    public string Translate { get; set; }
    public int Year { get; set; }
    public List<IFormFile> ImageCollection { get; set; } = new();

}