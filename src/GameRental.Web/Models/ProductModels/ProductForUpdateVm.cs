using System.ComponentModel.DataAnnotations;

namespace GameRental.Web.Models.ProductModels;

public class ProductForUpdateVm
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Введите имя")]
    [MaxLength(50, ErrorMessage = "Максимальная длина 50")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Введите описание товара")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Необходимо выбрать категории")]
    public int[] ProductCategoriesId { get; set; }
    
    [Required(ErrorMessage = "Необходимо ввести время прохождения")]
    public int TransitTime { get; set; }
    
    [Required(ErrorMessage = "Необходимо ввести перевод")]
    public string Translate { get; set; }
    
    [Required(ErrorMessage = "Необходимо ввести год выпуска")]
    public int Year { get; set; }

    public int[]? ImagesId { get; set; } 
    public List<ImageVm> Images { get; set; } = new();

    public List<IFormFile>? ImageCollection { get; set; } = new();
    public List<ProductCategoryVM>? AllCategories { get; set; }
}