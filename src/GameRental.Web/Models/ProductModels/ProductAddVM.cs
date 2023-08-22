using System.ComponentModel.DataAnnotations;

namespace GameRental.Web.Models.ProductModels;

public class ProductAddVM
{
    [Required(ErrorMessage = "Введите название игры")]
    [MaxLength(50, ErrorMessage = "Максимальная длина 50")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Введите описание товара")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Необходимо ввести время прохождения")]
    public int TransitTime { get; set; }
    
    [Required(ErrorMessage = "Необходимо ввести перевод")]
    public string Translate { get; set; }
    
    [Required(ErrorMessage = "Необходимо ввести год выпуска")]
    public int Year { get; set; }
    
    [Required(ErrorMessage = "Необходимо выбрать категории")]
    public int[] ProductCategoriesId { get; set; }

    public List<ProductCategoryVM> Categories { get; set; } = new();

    [Required(ErrorMessage = "Необходимо загрузить фотографии")]
    public List<IFormFile> ImageCollection { get; set; } = new();

}