using GameRental.Application.DTO.Product;

namespace GameRental.Application.DTO.Image;

public class ImageDTO
{
    public int Id { get; set; }
    public string ImgName { get; set; }
    public int ProductId { get; set; }
    public ProductDTO? Product { get; set; }
}